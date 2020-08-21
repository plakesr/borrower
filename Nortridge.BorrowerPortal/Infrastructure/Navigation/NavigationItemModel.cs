// <copyright file="NavigationItemModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Infrastructure.Navigation
{
    using System.Collections.ObjectModel;

    public class NavigationItemModel : INavigationItem
    {
        public NavigationItemModel(string name, string path, ReadOnlyCollection<SubNavigationItemModel> children)
        {
            this.Name = name;
            this.Path = path;
            this.Children = children;
        }

        public string Name { get; }

        public string Path { get; }

        public ReadOnlyCollection<SubNavigationItemModel> Children { get; }
    }
}
