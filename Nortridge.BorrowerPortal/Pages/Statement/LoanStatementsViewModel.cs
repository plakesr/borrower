// <copyright file="LoanStatementsViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Statement
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class LoanStatementsViewModel
    {
        [Obsolete("Autoconvert only", error: true)]
        public LoanStatementsViewModel()
        {
        }

        public LoanStatementsViewModel(
            ReadOnlyCollection<SelectListItem> loans,
            long loanId,
            ReadOnlyCollection<LoanStatementViewModel> loanStatements)
        {
            this.Loans = loans;
            this.LoanId = loanId;
            this.LoanStatements = loanStatements;
        }

        public ReadOnlyCollection<SelectListItem> Loans { get; }

        [BindRequired]
        public long LoanId { get; set; }

        public ReadOnlyCollection<LoanStatementViewModel> LoanStatements { get; }
    }
}
