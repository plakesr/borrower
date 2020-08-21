// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.PaymentHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Contacts;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.BorrowerPortal.Infrastructure;
    using Nortridge.NlsWebApi.Client;

    public class IndexModel : PageModel
    {
        private readonly ContactService contactService;
        private readonly LoanSettingsService loanSettingsService;
        private readonly LoanService loanService;
        private readonly IStringLocalizer<IndexModel> localizer;

        public IndexModel(
            ContactService contactService,
            LoanSettingsService loanSettingsService,
            LoanService loanService,
            IStringLocalizer<IndexModel> localizer)
        {
            this.contactService = contactService;
            this.loanSettingsService = loanSettingsService;
            this.loanService = loanService;
            this.localizer = localizer;
        }

        [BindProperty]
        public LoanPaymentHistoryViewModel PaymentHistory { get; set; }

        public async Task OnGetAsync() =>
             this.PaymentHistory = await this.Build();

        public async Task<IActionResult> OnPostAsync()
        {
            this.PaymentHistory = await this.Build(this.PaymentHistory.LoanId);

            return this.Page();
        }

        private static LoanPaymentHistoryItemViewModel MapItem(
            Dictionary<int, string> paymentMethods,
            Loanacct_Payment_HistoryDto paymentHistory)
        {
            var type = LoanPaymentType.Of(paymentHistory.Payment_Type);

            var result = new LoanPaymentHistoryItemViewModel(
                type == LoanPaymentType.Payment ? paymentHistory.Date_Paid : (DateTime?)null,
                paymentHistory.Date_Due,
                paymentHistory.Payment_Number.Apply(ToNullable),
                paymentHistory.Payment_Description,
                paymentHistory.Payment_Amount,
                paymentHistory.Transaction_Code.Apply(ToNullable),
                type == LoanPaymentType.Payment ? paymentMethods[paymentHistory.Payment_Method_No] : string.Empty,
                paymentHistory.Payment_Method_Reference,
                paymentHistory.Transaction_Reference_No);

            return result;
        }

        private static T? ToNullable<T>(T source)
            where T : struct
            =>
            source.Equals(default(T)) ? (T?)null : source;

        private async Task<LoanPaymentHistoryViewModel> Build(long? loanId = null)
        {
            var loans = await this.contactService.Loans(this.User.Id());

            var paymentMethods = await this.loanSettingsService.PaymentMethods()
                .Map(_ => _.ToDictionary(
                    pMethod => pMethod.Payment_Method_No,
                    pMethod => pMethod.Payment_Method_Code));

            var id = loanId ?? loans.First().Acctrefno;
            var paymentHistory = await this.loanService.PaymentHistory(id);

            return new LoanPaymentHistoryViewModel(
                loans: loans.Select(MapLoansList).ToReadOnly(),
                loanId: id,
                items: paymentHistory
                    .Select(_ => MapItem(paymentMethods, _))
                    .OrderByDescending(_ => _.TransactionReferenceNo)
                    .GroupBy(_ => _.TransactionReferenceNo)
                    .ToReadOnly());
        }

        private SelectListItem MapLoansList(LoanDto loan)
        {
            var nickname = loan.Borrower_Loan_Nickname.Empty() ?
                this.localizer["Nickname"] :
                loan.Borrower_Loan_Nickname;
            return new SelectListItem($"{loan.Loan_Number} ({nickname})", loan.Acctrefno.ToStringInv());
        }
    }
}
