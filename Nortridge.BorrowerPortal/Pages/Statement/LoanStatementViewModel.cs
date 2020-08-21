// <copyright file="LoanStatementViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Statement
{
    using System;

    public class LoanStatementViewModel
    {
        public LoanStatementViewModel(DateTime statementDate, DateTime? statementDueDate, double pastDueBalance, double currentBalance, double currentPayoff, long documentId)
        {
            this.StatementDate = statementDate;
            this.StatementDueDate = statementDueDate;
            this.PastDueBalance = pastDueBalance;
            this.CurrentBalance = currentBalance;
            this.CurrentPayoff = currentPayoff;
            this.DocumentId = documentId;
        }

        public DateTime StatementDate { get; }

        public DateTime? StatementDueDate { get; }

        public double PastDueBalance { get; }

        public double CurrentBalance { get; }

        public double CurrentPayoff { get; }

        public long DocumentId { get; }
    }
}
