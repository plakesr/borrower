// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.SubmitPayment
{
    using System.Linq;
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Commands;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Configuration;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Queries;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.BorrowerPortal.Core.UseCases.Loan.Payment;
    using static LanguageExt.Prelude;

    public class IndexModel : PageModel
    {
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IStringLocalizer<IndexModel> localizer;
        private readonly LoanService loanService;
        private readonly IPayixSettingsConfig payixSettings;
        private readonly WebPayService webPayService;
        private readonly PayixService payixService;
        private readonly PayixBuilder payixBuilder;

        private const string CreatedPaymentKey = "CreatedPayment";
        private const int PaymentMethodCard = -6;

        public IndexModel(
            IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher,
            IStringLocalizer<IndexModel> localizer,
            LoanService loanService,
            IPayixSettingsConfig payixSettings,
            WebPayService webPayService,
            PayixService payixService,
            PayixBuilder payixBuilder)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.localizer = localizer;
            this.loanService = loanService;
            this.payixSettings = payixSettings;
            this.webPayService = webPayService;
            this.payixService = payixService;
            this.payixBuilder = payixBuilder;
        }

        public string CreatedPayment { get; private set; }

        public bool RecordLocked { get; private set; }

        public SubmitPaymentForm Form { get; private set; }

        public async Task OnGetAsync()
        {
            var payment = await this.Build();
            this.Form = BuildInitialStep(payment, PayType.Nothing, default);
            this.CreatedPayment = this.TempData[CreatedPaymentKey] as string;
        }

        public static SubmitPaymentForm BuildInitialStep(SubmitPaymentViewModel payment, PayType payType, string webPayUrl)
        {
            return new SubmitPaymentForm(
                payment: payment,
                payType: PayType.Nothing,
                payixPaymentSetupAmount: default,
                payixPaymentCardsList: default,
                payixPaymentAddCards: default,
                step: PayixStep.InitialStep,
                webPayUrl: default);
        }

        public async Task<IActionResult> OnPostAsync(SubmitPaymentViewModel payment)
        {
            if (!this.ModelState.IsValid)
            {
                this.Form = BuildInitialStep(payment, PayType.Nothing, default);
                return this.Page();
            }

            if (payment.Method != SubmitPaymentMethod.Card)
            {
                this.Form = BuildInitialStep(payment, PayType.Nothing, default);
                var response = await this.SendCommand();

                return await response.Match(
                    Right: description => this.SuccessCheckResult(description).Apply(Task.FromResult),
                    Left: this.ErrorCheckResult);
            }

            var paymentUrls = await this.payixService.GetPaymentUrls(payment.LoanId);
            var payments = await this.Build().Map(_ => _.WithMethod(SubmitPaymentMethod.Card));

            var common = new PayixPaymentCommon(
                    token: default,
                    loanId: payment.LoanId,
                    nickName: payments.Loans.First(_ => _.Value == payment.LoanId.ToStringInv()).Text);

            this.Form = new SubmitPaymentForm(
                payment: payments,
                payType: paymentUrls.PayType,
                payixPaymentSetupAmount: default,
                payixPaymentCardsList: await this.payixBuilder.GetCardList(
                        urls: paymentUrls,
                        payment: payment,
                        common: common),
                payixPaymentAddCards: await this.payixBuilder.GetAddCards(
                        urls: paymentUrls,
                        payment: payment, 
                        common: common),
                step: await this.payixBuilder.GetStep(urls: paymentUrls, payment: payment),
                webPayUrl: paymentUrls.WebPayUrl);

            return this.Page();
        }

        private Task<IActionResult> ErrorCheckResult(CommonError<SubmitPaymentErrorType> error)
        {
            if (error.Type == SubmitPaymentErrorType.RecordLocked)
            {
                this.RecordLocked = true;
            }
            else if (error.Type == SubmitPaymentErrorType.CheckABANumber)
            {
                this.ModelState.AddModelError(
                    key: $"{nameof(this.Form.Payment.CheckABANumber)}",
                    this.localizer[error.Message]);
            }

            return this.RestorePage();
        }

        private IActionResult SuccessCheckResult(string automatedPaymentDescription)
        {
            this.TempData[CreatedPaymentKey] = automatedPaymentDescription;
            this.TempData.Keep();

            return this.RedirectToPage();
        }

        private async Task<IActionResult> RestorePage()
        {
            var payment = await this.Build().Map(_ => _.WithMethod(this.Form.Payment.Method));

            this.Form = BuildInitialStep(payment, payType: PayType.Nothing, default);

            var loan = this.Form.Payment.Loans.FirstOrDefault(_ => _.Value == this.Form.Payment.LoanId.ToStringInv());
            if (loan != null)
            {
                loan.Selected = true;
            }

            return this.Page();
        }

        private Task<SubmitPaymentViewModel> Build()
        {
            var query = new SubmitPaymentQuery(this.User.Id());
            return this.queryDispatcher.Ask<SubmitPaymentQuery, Task<SubmitPaymentViewModel>>(query);
        }

        private Task<Either<CommonError<SubmitPaymentErrorType>, string>> SendCommand()
        {
            var command = new SubmitPaymentCommand(this.Form.Payment);
            return this.commandDispatcher
                .Handle<SubmitPaymentCommand, Task<Either<CommonError<SubmitPaymentErrorType>, string>>>(command);
        }

        public async Task<IActionResult> OnPostPayixSetupAmountNewCardAsync(PayixPaymentAddCardsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.Form = new SubmitPaymentForm(
                    payment: default,
                    payType: PayType.Payix,
                    payixPaymentSetupAmount: default,
                    payixPaymentCardsList: default,
                    payixPaymentAddCards: await this.payixBuilder.AddCardStep(model.Common),
                    step: PayixStep.AddCard,
                    webPayUrl: default);
            }
            else
            {
                this.Form = new SubmitPaymentForm(
                    payment: default,
                    payType: PayType.Payix,
                    payixPaymentSetupAmount: this.payixBuilder.AmountStep(
                        common: model.Common,
                        amount: default,
                        reference: default),
                    payixPaymentCardsList: default,
                    payixPaymentAddCards: default,
                    step: PayixStep.SetupAmount,
                    webPayUrl: default);
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostPayixSetupAmountAsync(PayixPaymentCardsListViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.Form = new SubmitPaymentForm(
                    payment: default,
                    payType: PayType.Payix,
                    payixPaymentSetupAmount: this.payixBuilder.AmountStep(
                        common: model.Common,
                        amount: default,
                        reference: default),
                    payixPaymentCardsList: default,
                    payixPaymentAddCards: default,
                    step: PayixStep.SetupAmount,
                    webPayUrl: default);
            }
            else
            {
                this.Form = new SubmitPaymentForm(
                    payment: default,
                    payType: PayType.Payix,
                    payixPaymentSetupAmount: default,
                    payixPaymentCardsList: this.payixBuilder.CardListStep(
                        cards: await this.payixService.CardsList(model.Common.LoanId),
                        common: model.Common,
                        errorMessage: default),
                    payixPaymentAddCards: default,
                    step: PayixStep.GetCardList,
                    webPayUrl: default);
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostPayixAddNewCardAsync(PayixPaymentCardsListViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.Form = new SubmitPaymentForm(
                    payment: default,
                    payType: PayType.Payix,
                    payixPaymentSetupAmount: default,
                    payixPaymentCardsList: default,
                    payixPaymentAddCards: await this.payixBuilder.AddCardStep(model.Common),
                    step: PayixStep.AddCard,
                    webPayUrl: default);
            }
            else
            {
                this.Form = new SubmitPaymentForm(
                    payment: default,
                    payType: PayType.Payix,
                    payixPaymentSetupAmount: default,
                    payixPaymentCardsList: this.payixBuilder.CardListStep(
                        cards: await this.payixService.CardsList(model.Common.LoanId),
                        common: model.Common,
                        errorMessage: default),
                    payixPaymentAddCards: default,
                    step: PayixStep.GetCardList,
                    webPayUrl: default);
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostPayixSubmitPaymentAsync(PayixPaymentSetupAmountViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.Form = new SubmitPaymentForm(
                    payment: default,
                    payType: PayType.Payix,
                    payixPaymentSetupAmount: this.payixBuilder.AmountStep(
                        common: model.Common,
                        amount: model.Amount,
                        reference: model.Reference),
                    payixPaymentCardsList: default,
                    payixPaymentAddCards: default,
                    step: PayixStep.SetupAmount,
                    webPayUrl: default);
            }
            else
            {
                var response = await this.payixService.SubmitPayment(
                    new PayixPaymentParam(
                        amount: model.Amount,
                        reference: model.Reference,
                        token: model.Common.Token,
                        loanId: model.Common.LoanId,
                        PaymentMethodCard));

                this.Form = await response.MatchAsync(
                        LeftAsync: async description =>
                        {
                             return new SubmitPaymentForm(
                                payment: default,
                                payType: PayType.Payix,
                                payixPaymentSetupAmount: default,
                                payixPaymentCardsList: this.payixBuilder.CardListStep(
                                    cards: await this.payixService.CardsList(model.Common.LoanId),
                                    common: model.Common,
                                    errorMessage: description),
                                payixPaymentAddCards: default,
                                step: PayixStep.GetCardList,
                                webPayUrl: default);
                        },
                        Right: _ =>
                        {
                            return new SubmitPaymentForm(
                                payment: default,
                                payType: PayType.Payix,
                                payixPaymentSetupAmount: default,
                                payixPaymentCardsList: default,
                                payixPaymentAddCards: default,
                                step: PayixStep.Successful,
                                webPayUrl: default);
                        });
            }

            return this.Page();
        }
    }
}
