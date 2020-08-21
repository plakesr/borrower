// <copyright file="TopNavigationViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Components
{
    using System.Collections.ObjectModel;
    using Nortridge.BorrowerPortal.Infrastructure.Navigation;

    public class TopNavigationViewModel
    {
        public TopNavigationViewModel(
            ReadOnlyCollection<NavigationItemModel> items,
            NavigationItemModel activeItem)
        {
            this.Items = items;
            this.ActiveItem = activeItem;
        }

        public ReadOnlyCollection<NavigationItemModel> Items { get; }

        public NavigationItemModel ActiveItem { get; }
    }
}
