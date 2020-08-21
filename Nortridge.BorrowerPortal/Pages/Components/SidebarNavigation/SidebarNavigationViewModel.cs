// <copyright file="SidebarNavigationViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Components
{
    using System.Collections.ObjectModel;
    using Nortridge.BorrowerPortal.Infrastructure.Navigation;

    public class SidebarNavigationViewModel
    {
        public SidebarNavigationViewModel(
            ReadOnlyCollection<SubNavigationItemModel> items,
            SubNavigationItemModel activeItem)
        {
            this.Items = items;
            this.ActiveItem = activeItem;
        }

        public ReadOnlyCollection<SubNavigationItemModel> Items { get; }

        public SubNavigationItemModel ActiveItem { get; }
    }
}
