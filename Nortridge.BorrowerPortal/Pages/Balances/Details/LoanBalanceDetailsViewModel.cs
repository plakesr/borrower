// <copyright file="LoanBalanceDetailsViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Balances.Details
{
    using System;

    public class LoanBalanceDetailsViewModel
    {
        public LoanBalanceDetailsViewModel(
            string number,
            string nickname,
            DateTime openDate,
            DateTime? maturityDate,
            double loanAmount,
            DateTime balanceDate,
            double principal,
            double interest,
            double fees,
            double lateCharges,
            double otherCharges,
            double totalBalance,
            double currentImpound,
            DateTime interestAccruedThrough,
            double perdiemInterest,
            double nextPrincipalBilling,
            DateTime? nextPrincipalBillingDueDate,
            double nextInterestBilling,
            DateTime? nextInterestBillingDueDate,
            double nextTotalPayment,
            double nextTotalBilling,
            double? lastPayment,
            DateTime? lastPaymentReceivedDate,
            double totalPastDue,
            double totalCurrentDue,
            string loanType,
            double currentRate,
            int daysPastDue)
        {
            this.Number = number;
            this.Nickname = nickname;
            this.OpenDate = openDate;
            this.MaturityDate = maturityDate;
            this.LoanAmount = loanAmount;
            this.BalanceDate = balanceDate;
            this.Principal = principal;
            this.Interest = interest;
            this.Fees = fees;
            this.LateCharges = lateCharges;
            this.OtherCharges = otherCharges;
            this.TotalBalance = totalBalance;
            this.CurrentImpound = currentImpound;
            this.InterestAccruedThrough = interestAccruedThrough;
            this.PerdiemInterest = perdiemInterest;
            this.NextPrincipalBilling = nextPrincipalBilling;
            this.NextPrincipalBillingDueDate = nextPrincipalBillingDueDate;
            this.NextInterestBilling = nextInterestBilling;
            this.NextInterestBillingDueDate = nextInterestBillingDueDate;
            this.NextTotalPayment = nextTotalPayment;
            this.NextTotalBilling = nextTotalBilling;
            this.LastPayment = lastPayment;
            this.LastPaymentReceivedDate = lastPaymentReceivedDate;
            this.TotalPastDue = totalPastDue;
            this.TotalCurrentDue = totalCurrentDue;
            this.LoanType = loanType;
            this.CurrentRate = currentRate;
            this.DaysPastDue = daysPastDue;
        }

        public string Number { get; }

        public string Nickname { get; }

        public DateTime OpenDate { get; }

        public DateTime? MaturityDate { get; }

        public double LoanAmount { get; }

        public DateTime BalanceDate { get; }

        public double Principal { get; }

        public double Interest { get; }

        public double Fees { get; }

        public double LateCharges { get; }

        public double OtherCharges { get; }

        public double TotalBalance { get; }

        public double CurrentImpound { get; }

        public DateTime InterestAccruedThrough { get; }

        public double PerdiemInterest { get; }

        public double NextPrincipalBilling { get; }

        public DateTime? NextPrincipalBillingDueDate { get; }

        public double NextInterestBilling { get; }

        public DateTime? NextInterestBillingDueDate { get; }

        public double NextTotalPayment { get; }

        public double NextTotalBilling { get; }

        public double? LastPayment { get; }

        public DateTime? LastPaymentReceivedDate { get; }

        public double TotalPastDue { get; }

        public double TotalCurrentDue { get; }

        public string LoanType { get; }

        public double CurrentRate { get; }

        public int DaysPastDue { get; }
    }
}
