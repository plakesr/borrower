// <copyright file="Default.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Components
{
    using Microsoft.AspNetCore.Mvc;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Infrastructure.Navigation;

    public class SidebarNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var currentPage = this.RouteData.CurrentPage();
            var navitem = NavigationBuilder.ActiveNavigationItem(currentPage);

            var result = new SidebarNavigationViewModel(
                navitem.Children,
                NavigationBuilder.ActiveSubNavigationItem(navitem, currentPage));

            return this.View(result);
        }
    }
}
