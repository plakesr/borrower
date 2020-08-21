// <copyright file="ChangePasswordViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account.ChangePassword
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Constants;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Validation;

    public class ChangePasswordViewModel
    {
        [Obsolete("Autoconvert only", error: true)]
        public ChangePasswordViewModel()
        {
        }

        public ChangePasswordViewModel(string currentPassword, string newPassword, string confirmPassword)
        {
            this.CurrentPassword = currentPassword;
            this.NewPassword = newPassword;
            this.ConfirmPassword = confirmPassword;
        }

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(12)]
        [DataType(DataType.Password)]
        [RegularExpression(ValidationConstants.Password)]
        [NotEqualTo(nameof(CurrentPassword))]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
