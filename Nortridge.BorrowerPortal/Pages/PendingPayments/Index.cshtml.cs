// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.PendingPayments
{
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
        public LoanPendingPaymentsViewModel PendingPayments { get; set; }

        public async Task OnGetAsync() =>
             this.PendingPayments = await this.Build();

        public async Task<IActionResult> OnPostAsync()
        {
            this.PendingPayments = await this.Build(this.PendingPayments.LoanId);

            return this.Page();
        }

        private static LoanPendingPaymentItemViewModel MapPendingTransaction(
            ICollection<Loan_Payment_MethodDto> paymentMethods,
            Pending_TransactionsDto transaction)
            =>
            new LoanPendingPaymentItemViewModel(
                executionDate: transaction.Execution_Date,
                effectiveDate: transaction.Effective_Date,
                type: transaction.Transaction_Type,
                typeDescription: transaction.Transaction_Description,
                amount: transaction.Transaction_Amount,
                paymentMethod: PaymentMethod(paymentMethods, transaction.Payment_Method_No),
                paymentReference: transaction.Payment_Method_Reference,
                userDef1: transaction.Userdef01,
                userDef2: transaction.Userdef02,
                userDef3: transaction.Userdef03,
                userDef4: transaction.Userdef04,
                userDef5: transaction.Userdef05,
                userReference: transaction.User_Reference,
                aCHBatchNumber: transaction.Ach_Batch_No,
                aCHTraceNumber: transaction.Ach_Trace_Number);

        private static string PaymentMethod(
            ICollection<Loan_Payment_MethodDto> paymentMethods,
            int paymentMethodId)
            =>
            paymentMethods.SingleOrDefault(_ =>
                _.Payment_Method_No == paymentMethodId)?.Payment_Method_Code ?? paymentMethodId.ToStringInv();

        private async Task<LoanPendingPaymentsViewModel> Build(long? loanId = null)
        {
            var loans = await this.contactService.Loans(this.User.Id());

            var id = loanId ?? loans.First().Acctrefno;
            var pendingPayments = await this.loanService.PendingTransactions(id);

            var paymentMethods = await this.loanSettingsService.PaymentMethods();

            var items = pendingPayments
                .Select(_ => MapPendingTransaction(paymentMethods, _))
                .ToReadOnly();

            return new LoanPendingPaymentsViewModel(
                loans: loans.Select(MapLoansList).ToReadOnly(),
                loanId: id,
                items: items);
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
