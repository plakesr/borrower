// <copyright file="LoanPendingPaymentItemViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.PendingPayments
{
    using System;

    public class LoanPendingPaymentItemViewModel
    {
        public LoanPendingPaymentItemViewModel(
            DateTime executionDate,
            DateTime effectiveDate,
            int type,
            string typeDescription,
            double amount,
            string paymentMethod,
            string paymentReference,
            string userDef1,
            string userDef2,
            string userDef3,
            string userDef4,
            string userDef5,
            string userReference,
            int aCHBatchNumber,
            string aCHTraceNumber)
        {
            this.ExecutionDate = executionDate;
            this.EffectiveDate = effectiveDate;
            this.Type = type;
            this.TypeDescription = typeDescription;
            this.Amount = amount;
            this.PaymentMethod = paymentMethod;
            this.PaymentReference = paymentReference;
            this.UserDef1 = userDef1;
            this.UserDef2 = userDef2;
            this.UserDef3 = userDef3;
            this.UserDef4 = userDef4;
            this.UserDef5 = userDef5;
            this.UserReference = userReference;
            this.ACHBatchNumber = aCHBatchNumber;
            this.ACHTraceNumber = aCHTraceNumber;
        }

        public DateTime ExecutionDate { get; }

        public DateTime EffectiveDate { get; }

        public int Type { get; }

        public string TypeDescription { get; }

        public double Amount { get; }

        public string PaymentMethod { get; }

        public string PaymentReference { get; }

        public string UserDef1 { get; }

        public string UserDef2 { get; }

        public string UserDef3 { get; }

        public string UserDef4 { get; }

        public string UserDef5 { get; }

        public string UserReference { get; }

        public int ACHBatchNumber { get; }

        public string ACHTraceNumber { get; }
    }
}
