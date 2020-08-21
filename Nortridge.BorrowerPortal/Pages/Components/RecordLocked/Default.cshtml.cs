// <copyright file="Default.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal
{
    using Microsoft.AspNetCore.Mvc;

    public class RecordLockedViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => this.View();
    }
}
