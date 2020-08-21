// <copyright file="LoanPaymentHistoryItemViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.PaymentHistory
{
    using System;

    public class LoanPaymentHistoryItemViewModel
    {
        public LoanPaymentHistoryItemViewModel(
            DateTime? datePaid,
            DateTime? dueDate,
            int? paymentNumber,
            string type,
            double paymentAmount,
            int? transactionCode,
            string paymentMethod,
            string paymentMethodRef,
            int transactionReferenceNo)
        {
            this.DatePaid = datePaid;
            this.DueDate = dueDate;
            this.PaymentNumber = paymentNumber;
            this.Type = type;
            this.PaymentAmount = paymentAmount;
            this.TransactionCode = transactionCode;
            this.PaymentMethod = paymentMethod;
            this.PaymentMethodRef = paymentMethodRef;
            this.TransactionReferenceNo = transactionReferenceNo;
        }

        public DateTime? DatePaid { get; }

        public DateTime? DueDate { get; }

        public int? PaymentNumber { get; }

        public string Type { get; }

        public double PaymentAmount { get; }

        public int? TransactionCode { get; }

        public string PaymentMethod { get; }

        public string PaymentMethodRef { get; }

        public int TransactionReferenceNo { get; }
    }
}
