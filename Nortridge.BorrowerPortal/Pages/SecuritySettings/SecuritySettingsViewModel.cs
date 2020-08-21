// <copyright file="SecuritySettingsViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.SecuritySettings
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SecuritySettingsViewModel
    {
        [Obsolete("Autoconvert only", error: true)]
        public SecuritySettingsViewModel()
        {
        }

        public SecuritySettingsViewModel(
            ReadOnlyCollection<SelectListItem> questions1,
            ReadOnlyCollection<SelectListItem> questions2,
            string answer1,
            string answer2)
        {
            this.Questions1 = questions1;
            this.Questions2 = questions2;
            this.Answer1 = answer1;
            this.Answer2 = answer2;
        }

        public ReadOnlyCollection<SelectListItem> Questions1 { get; }

        public ReadOnlyCollection<SelectListItem> Questions2 { get; }

        public string Question1 { get; set; }

        public string Question2 { get; set; }

        [MaxLength(255)]
        [Required]
        public string Answer1 { get; set; }

        [MaxLength(255)]
        [Required]
        public string Answer2 { get; set; }
    }
}
