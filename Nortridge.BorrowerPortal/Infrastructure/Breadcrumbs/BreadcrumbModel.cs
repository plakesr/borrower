// <copyright file="BreadcrumbModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Infrastructure.Breadcrumbs
{
    public class BreadcrumbModel
    {
        public BreadcrumbModel(string name, string path)
        {
            this.Name = name;
            this.Path = path;
        }

        public string Name { get; }

        public string Path { get; }
    }
}
