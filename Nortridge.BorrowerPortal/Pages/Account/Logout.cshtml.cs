// <copyright file="Logout.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Nortridge.BorrowerPortal.Core.Auth;

    public class LogoutModel : PageModel
    {
        private readonly AuthService authService;

        public LogoutModel(AuthService authService)
        {
            this.authService = authService;
        }

        [AllowAnonymous]
        public IActionResult OnGet() =>
            this.User.Identity.IsAuthenticated ?
                this.RedirectToPage("/Index") :
                this.RedirectToPage("/Account/Login/Index");

        public async Task<IActionResult> OnPostAsync()
        {
            await this.authService.SignOut();

            return this.RedirectToPage("/Account/Login/Index");
        }
    }
}
