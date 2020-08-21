// <copyright file="Default.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Components
{
    using Microsoft.AspNetCore.Mvc;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Infrastructure.Navigation;

    public class TopNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var result = new TopNavigationViewModel(
                NavigationBuilder.TopNavigation,
                NavigationBuilder.ActiveNavigationItem(this.RouteData.CurrentPage()));

            return this.View(result);
        }
    }
}
