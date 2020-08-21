// <copyright file="Default.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Components
{
    using Microsoft.AspNetCore.Mvc;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Infrastructure.Navigation;

    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var result = new HeaderViewModel(
                NavigationBuilder.Home.Path,
                this.UserClaimsPrincipal.UserName());

            return this.View(result);
        }
    }
}
