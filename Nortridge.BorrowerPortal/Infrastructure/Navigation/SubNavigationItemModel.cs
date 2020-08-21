// <copyright file="SubNavigationItemModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Infrastructure.Navigation
{
    using System.Collections.ObjectModel;

    public class SubNavigationItemModel : INavigationItem
    {
        public SubNavigationItemModel(
            string name,
            string path,
            string icon,
            ReadOnlyCollection<string> relatedPaths)
        {
            this.Name = name;
            this.Path = path;
            this.Icon = icon;
            this.RelatedPaths = relatedPaths;
        }

        public string Name { get; }

        public string Path { get; }

        public string Icon { get; }

        public ReadOnlyCollection<string> RelatedPaths { get; }
    }
}
