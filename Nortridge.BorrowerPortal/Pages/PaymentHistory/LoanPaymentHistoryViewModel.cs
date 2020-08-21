// <copyright file="LoanPaymentHistoryViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.PaymentHistory
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class LoanPaymentHistoryViewModel
    {
        [Obsolete("Autoconvert only", error: true)]
        public LoanPaymentHistoryViewModel()
        {
        }

        public LoanPaymentHistoryViewModel(
            ReadOnlyCollection<SelectListItem> loans,
            long loanId,
            ReadOnlyCollection<IGrouping<int, LoanPaymentHistoryItemViewModel>> items)
        {
            this.Loans = loans;
            this.LoanId = loanId;
            this.Items = items;
        }

        public ReadOnlyCollection<SelectListItem> Loans { get; }

        [BindRequired]
        public long LoanId { get; set; }

        public ReadOnlyCollection<IGrouping<int, LoanPaymentHistoryItemViewModel>> Items { get; }
    }
}
