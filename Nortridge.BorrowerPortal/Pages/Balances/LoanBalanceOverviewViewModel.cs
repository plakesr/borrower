// <copyright file="LoanBalanceOverviewViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Balances
{
    using System;

    public class LoanBalanceOverviewViewModel
    {
        public LoanBalanceOverviewViewModel(
            long id,
            string number,
            string nickname,
            DateTime? maturityDate,
            double noteAmount,
            double payoffBalance,
            double pastDue)
        {
            this.Id = id;
            this.Number = number;
            this.Nickname = nickname;
            this.MaturityDate = maturityDate;
            this.NoteAmount = noteAmount;
            this.PayoffBalance = payoffBalance;
            this.PastDue = pastDue;
        }

        public long Id { get; }

        public string Number { get; }

        public string Nickname { get; }

        public DateTime? MaturityDate { get; }

        public double NoteAmount { get; }

        public double PayoffBalance { get; }

        public double PastDue { get; }
    }
}
