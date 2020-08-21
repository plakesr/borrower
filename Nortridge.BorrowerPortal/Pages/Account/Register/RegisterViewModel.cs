// <copyright file="RegisterViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account.Register
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Nortridge.BorrowerPortal.Core.Account.Interfaces;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Constants;

    public class RegisterViewModel : IRegisterViewModel
    {
        [Obsolete("Auto-convert only", error: true)]
        public RegisterViewModel()
        {
        }

        public RegisterViewModel(
            string username,
            string newPassword,
            string confirmPassword,
            ReadOnlyCollection<SelectListItem> securityQuestion1List,
            string securityQuestion1,
            string securityQuestion1Answer,
            ReadOnlyCollection<SelectListItem> securityQuestion2List,
            string securityQuestion2,
            string securityQuestion2Answer,
            string firstName,
            string lastName,
            string email,
            DateTime? dateOfBirth,
            bool alwaysTrustThisComputer)
        {
            this.Username = username;
            this.Password = newPassword;
            this.ConfirmPassword = confirmPassword;
            this.SecurityQuestion1List = securityQuestion1List;
            this.SecurityQuestion1 = securityQuestion1;
            this.SecurityQuestion1Answer = securityQuestion1Answer;
            this.SecurityQuestion2List = securityQuestion2List;
            this.SecurityQuestion2 = securityQuestion2;
            this.SecurityQuestion2Answer = securityQuestion2Answer;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.DateOfBirth = dateOfBirth;
            this.AlwaysTrustThisComputer = alwaysTrustThisComputer;
        }

        [Required]
        [MaxLength(25)]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(12)]
        [DataType(DataType.Password)]
        [RegularExpression(ValidationConstants.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public ReadOnlyCollection<SelectListItem> SecurityQuestion1List { get; }

        [Required]
        public string SecurityQuestion1 { get; set; }

        [Required]
        [MaxLength(255)]
        public string SecurityQuestion1Answer { get; set; }

        public ReadOnlyCollection<SelectListItem> SecurityQuestion2List { get; }

        [Required]
        public string SecurityQuestion2 { get; set; }

        [Required]
        [MaxLength(255)]
        public string SecurityQuestion2Answer { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.WordsAndWhispaceAndSpecialSymbolsWith1Up50Length)] //ErrorMessage = "Only the next characters are allowed a-z (A-Z) '-~!#&. and whitespace. Enter from 1 to 50 symbols."
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.WordsAndWhispaceAndSpecialSymbolsWith1Up50Length)] //ErrorMessage = "Only the next characters are allowed a-z (A-Z) '-~!#&. and whitespace. Enter from 1 to 50 symbols."
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(ValidationConstants.Email)]
        public string Email { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        public bool AlwaysTrustThisComputer { get; set; }
    }
}
