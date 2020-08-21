// <copyright file="INavigationItem.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Infrastructure.Navigation
{
    public interface INavigationItem
    {
        string Name { get; }

        string Path { get; }
    }
}
