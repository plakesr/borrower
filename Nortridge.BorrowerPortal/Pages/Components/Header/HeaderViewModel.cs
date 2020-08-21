// <copyright file="HeaderViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Components
{
    public class HeaderViewModel
    {
        public HeaderViewModel(string logoPath, string userName)
        {
            this.LogoPath = logoPath;
            this.UserName = userName;
        }

        public string LogoPath { get; }

        public string UserName { get; }
    }
}
