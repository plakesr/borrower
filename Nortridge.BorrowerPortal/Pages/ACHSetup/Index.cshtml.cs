// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.ACHSetup
{
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Commands;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Pages;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Queries;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.BorrowerPortal.Core.Loans.AutomatedPayments;
    using Nortridge.BorrowerPortal.Core.UseCases.Loan.AutomatedPayment;
    using static LanguageExt.Prelude;

    public class IndexModel : PageModelBase
    {
        private const string CreatedAutomatedPaymentKey = "CreatedAutomatedPayment";

        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IStringLocalizer<IndexModel> localizer;
        private readonly LoanService loanService;

        public IndexModel(
            IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher,
            IStringLocalizer<IndexModel> localizer,
            LoanService loanService)
        {
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.localizer = localizer;
            this.loanService = loanService;
        }

        [BindProperty]
        public AutomatedPaymentViewModel AutomatedPayment { get; set; }

        public string CreatedAutomatedPayment { get; private set; }

        public bool RecordLocked { get; private set; }

        public async Task OnGetAsync()
        {
            this.AutomatedPayment = await this.Build();
            this.CreatedAutomatedPayment = this.TempData[CreatedAutomatedPaymentKey] as string;
        }

        public Task<IActionResult> OnPostAsync()
        {
            this.ModelState.Remove($"{nameof(this.AutomatedPayment)}.{nameof(this.AutomatedPayment.NextPaymentDate)}");
            this.ClearValidationErrors();

            return this.RestorePage();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return await this.RestorePage();
            }

            var isNextPaymentDateValid = await this.IsNextPaymentDateValid();
            if (!isNextPaymentDateValid)
            {
                this.ModelState.AddModelError(
                    key: $"{nameof(this.AutomatedPayment)}.{nameof(this.AutomatedPayment.NextPaymentDate)}",
                    string.Empty);
                return await this.RestorePage();
            }

            var response = await this.SendCommand();
            return await response.Match(
                Right: description => this.SuccessResult(description).Apply(Task.FromResult),
                Left: this.ErrorResult);
        }

        private Task<IActionResult> ErrorResult(CommonError<AutomatedPaymentSetupErrorType> error)
        {
            if (error.Type == AutomatedPaymentSetupErrorType.RecordLocked)
            {
                this.RecordLocked = true;
            }
            else if (error.Type == AutomatedPaymentSetupErrorType.ABANumber)
            {
                this.ModelState.AddModelError(
                    key: $"{nameof(this.AutomatedPayment)}.{nameof(this.AutomatedPayment.ABANumber)}",
                    this.localizer[error.Message]);
            }

            return this.RestorePage();
        }

        private IActionResult SuccessResult(string automatedPaymentDescription)
        {
            this.TempData[CreatedAutomatedPaymentKey] = automatedPaymentDescription;
            this.TempData.Keep();

            return this.RedirectToPage();
        }

        private async Task<IActionResult> RestorePage()
        {
            this.AutomatedPayment = await this.Build().Map(_ => _.WithSetupItem(this.AutomatedPayment.SetupItem));
            return this.Page();
        }

        private Task<AutomatedPaymentViewModel> Build()
        {
            var query = new AutomatedPaymentSetupQuery(this.User.Id(), Optional(this.AutomatedPayment?.LoanId));
            return this.queryDispatcher.Ask<AutomatedPaymentSetupQuery, Task<AutomatedPaymentViewModel>>(query);
        }

        private Task<Either<CommonError<AutomatedPaymentSetupErrorType>, string>> SendCommand()
        {
            var command = new AutomatedPaymentSetupCommand(this.AutomatedPayment);
            return this.commandDispatcher
                .Handle<AutomatedPaymentSetupCommand, Task<Either<CommonError<AutomatedPaymentSetupErrorType>, string>>>(command);
        }

        private async Task<bool> IsNextPaymentDateValid()
        {
            if (this.AutomatedPayment.SetupItem != AutomatedPaymentSetupItem.SpecificPaymentAmount)
            {
                return true;
            }

            var loan = await this.loanService.One(this.AutomatedPayment.LoanId);
            var result = AutomatedPaymentService.IsNextPyamentDateValid(loan, this.AutomatedPayment.NextPaymentDate);
            return result;
        }
    }
}
