// <copyright file="LoanPendingPaymentsViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.PendingPayments
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class LoanPendingPaymentsViewModel
    {
        [Obsolete("Autoconvert only", error: true)]
        public LoanPendingPaymentsViewModel()
        {
        }

        public LoanPendingPaymentsViewModel(
            ReadOnlyCollection<SelectListItem> loans,
            long loanId,
            ReadOnlyCollection<LoanPendingPaymentItemViewModel> items)
        {
            this.Loans = loans;
            this.LoanId = loanId;
            this.Items = items;
        }

        public ReadOnlyCollection<SelectListItem> Loans { get; }

        [BindRequired]
        public long LoanId { get; set; }

        public ReadOnlyCollection<LoanPendingPaymentItemViewModel> Items { get; }
    }
}
