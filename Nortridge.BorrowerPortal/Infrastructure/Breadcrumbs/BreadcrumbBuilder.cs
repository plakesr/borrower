// <copyright file="BreadcrumbBuilder.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Infrastructure.Breadcrumbs
{
    using System.Collections.Generic;
    using LanguageExt;
    using Nortridge.BorrowerPortal.Infrastructure.Navigation;

    public static class BreadcrumbBuilder
    {
        public static IEnumerable<BreadcrumbModel> Build(string path)
        {
            yield return NavigationBuilder.Home.Apply(MapNavigationItem);

            var navItem = NavigationBuilder.ActiveNavigationItem(path);
            yield return MapNavigationItem(navItem);

            var subNavItem = NavigationBuilder.ActiveSubNavigationItem(navItem, path);
            yield return MapNavigationItem(subNavItem);
        }

        private static BreadcrumbModel MapNavigationItem(INavigationItem navItem) =>
            new BreadcrumbModel(navItem.Name, navItem.Path);
    }
}
