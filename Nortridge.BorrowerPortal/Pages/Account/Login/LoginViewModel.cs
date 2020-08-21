// <copyright file="LoginViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Nortridge.BorrowerPortal.Core.Auth.Interfaces;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Validation;

    public class LoginViewModel : ILoginViewModel
    {
        [Obsolete("Auto-convert only", error: true)]
        public LoginViewModel()
        {
        }

        public LoginViewModel(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        [Required]
        [MaxLength(25)]
        public string Username { get; set; }

        [RequiredExclHandler(handler: "ForgotPassword", errorMessage: "The Password field is required.")]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
