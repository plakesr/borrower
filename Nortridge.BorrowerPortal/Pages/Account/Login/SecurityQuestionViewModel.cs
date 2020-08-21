// <copyright file="SecurityQuestionViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Nortridge.BorrowerPortal.Core.Auth.Interfaces;

    public class SecurityQuestionViewModel : ISecurityQuestionViewModel
    {
        [Obsolete("Auto-convert only", error: true)]
        public SecurityQuestionViewModel()
        {
        }

        public SecurityQuestionViewModel(string username, string question, bool hintNumber)
        {
            this.Username = username;
            this.Question = question;
            this.AlwaysTrust = true;
            this.HintNumber = hintNumber;
        }

        [MaxLength(25)]
        [Required]
        public string Username { get; set; }

        public string Question { get; }

        [MaxLength(255)]
        [Required]
        public string Answer { get; set; }

        public bool AlwaysTrust { get; set; }

        public bool HintNumber { get; set; }
    }
}
