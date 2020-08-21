// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Statement
{
    using System.Collections.ObjectModel;
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

        [BindProperty]
        public LoanStatementsViewModel LoanStatements { get; set; }

        public async Task OnGetAsync() =>
            this.LoanStatements = await this.Build();

        public async Task<IActionResult> OnPostAsync()
        {
            var loanId = this.LoanStatements.LoanId;
            this.LoanStatements = await this.Build(loanId);

            return this.Page();
        }

        private static LoanStatementsViewModel Map(
            ReadOnlyCollection<SelectListItem> loans,
            long? loanId,
            ReadOnlyCollection<LoanStatementViewModel> loanStatements)
        {
            var id = loanId ?? default;
            return new LoanStatementsViewModel(
                loans: loans.ToReadOnly(),
                loanId: id,
                loanStatements: loanStatements);
        }

        private static LoanStatementViewModel MapStatement(LoanStatementRowVM statement)
            =>
            new LoanStatementViewModel(
                statementDate: statement.Statement_Date,
                statementDueDate: statement.Statement_Due_Date,
                pastDueBalance: statement.Total_Past_Due_Balance,
                currentBalance: statement.Total_Current_Due_Balance,
                currentPayoff: statement.Current_Payoff_Balance,
                documentId: statement.Statement_Row_Id);

        private SelectListItem MapLoansList(LoanDto loan)
        {
            var nickname = loan.Borrower_Loan_Nickname.Empty() ?
                this.localizer["Nickname"] :
                loan.Borrower_Loan_Nickname;
            return new SelectListItem($"{loan.Loan_Number} ({nickname})", loan.Acctrefno.ToStringInv());
        }

        private async Task<LoanStatementsViewModel> Build(long? loanId = null)
        {
            var loans = await this.contactService.Loans(this.User.Id());

            var id = loanId ?? loans.First().Acctrefno;
            var statements = await this.loanService.Statements(id);

            return Map(
                loans.Select(MapLoansList).ToReadOnly(),
                id,
                statements.Select(MapStatement).ToReadOnly());
        }
    }
}
