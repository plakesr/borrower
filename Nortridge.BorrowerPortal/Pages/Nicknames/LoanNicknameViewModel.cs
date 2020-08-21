// <copyright file="LoanNicknameViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Nicknames
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class SecuritySettingsViewModel
    {
        [Obsolete("Autoconvert only", error: true)]
        public SecuritySettingsViewModel()
        {
        }

        public SecuritySettingsViewModel(long loanId, string loanNumber, string accountDescription, string nickname)
        {
            this.LoanId = loanId;
            this.LoanNumber = loanNumber;
            this.AccountDescription = accountDescription;
            this.Nickname = nickname;
        }

        [BindRequired]
        [HiddenInput]
        public long LoanId { get; set; }

        public string LoanNumber { get; }

        public string AccountDescription { get; }

        [MaxLength(50)]
        public string Nickname { get; set; }
    }
}
