// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account.ForgotPassword
{
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Commands;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Constants;
    using Nortridge.BorrowerPortal.Core.UseCases.Account;
    using static LanguageExt.Prelude;

    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ICommandDispatcher commandDispatcher;

        public IndexModel(
            ICommandDispatcher commandDispatcher)
        {
            this.commandDispatcher = commandDispatcher;
        }

        [BindProperty]
        public ForgotPasswordViewModel Form { get; set; }

        public Option<CommonError<ForgotPasswordErrorType>> Result { get; private set; } = None;

        public void OnGet() =>
            this.Form = new ForgotPasswordViewModel(default);

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var response = await this.SendCommand();
            return response.Match<IActionResult>(
                Right: _ =>
                {
                    this.TempData[AccountConstants.ForgotPasswordSent] = true;
                    this.TempData.Keep();

                    return this.RedirectToPage("/Account/Login/Index");
                },
                Left: error =>
                {
                    this.Result = error;
                    return this.Page();
                });
        }

        private Task<Either<CommonError<ForgotPasswordErrorType>, Unit>> SendCommand()
        {
            var command = new ForgotPasswordCommand(this.Form.Email);
            return this.commandDispatcher
                .Handle<ForgotPasswordCommand, Task<Either<CommonError<ForgotPasswordErrorType>, Unit>>>(command);
        }
    }
}
