// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Contacts;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.BorrowerPortal.Pages.Balances;
    using Nortridge.NlsWebApi.Client;

    public class IndexModel : PageModel
    {
        private readonly ContactService contactService;
        private readonly LoanService loanService;
        private readonly IStringLocalizer<IndexModel> localizer;

        public IndexModel(
            ContactService contactService,
            LoanService loanService,
            IStringLocalizer<IndexModel> localizer)
        {
            this.contactService = contactService;
            this.loanService = loanService;
            this.localizer = localizer;
        }

        public ReadOnlyCollection<LoanBalanceOverviewViewModel> Loans { get; private set; }

        public async Task OnGetAsync()
        {
            var loans = await this.contactService.Loans(this.User.Id());

            this.Loans = loans.Select(Map).ToReadOnly();
        }

        private LoanBalanceOverviewViewModel Map(LoanDto loan) =>
            new LoanBalanceOverviewViewModel(
                loan.Acctrefno,
                loan.Loan_Number,
                loan.Borrower_Loan_Nickname.Empty() ?
                    this.localizer["Nickname"] :
                    loan.Borrower_Loan_Nickname,
                loan.Curr_Maturity_Date,
                loan.Current_Note_Amount,
                loan.Current_Payoff_Balance,
                loan.Total_Past_Due_Balance);
    }
}
