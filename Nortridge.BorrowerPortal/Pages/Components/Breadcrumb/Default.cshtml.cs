// <copyright file="Default.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Components
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Infrastructure.Breadcrumbs;

    public class BreadcrumbViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(BreadcrumbModel current = null)
        {
            var items = BreadcrumbBuilder.Build(this.RouteData.CurrentPage());
            var result = current != null ? items.Append(current) : items;

            return this.View(result.ToReadOnly());
        }
    }
}
