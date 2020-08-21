// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account.ResetPassword
{
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Nortridge.BorrowerPortal.Core.Account;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Commands;
    using Nortridge.BorrowerPortal.Core.UseCases.Account;
    using static LanguageExt.Prelude;

    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly AccountValidator accountValidator;
        private readonly AccountTokenValidator accountTokenValidator;

        public IndexModel(
            ICommandDispatcher commandDispatcher,
            AccountValidator accountValidator,
            AccountTokenValidator accountTokenValidator)
        {
            this.commandDispatcher = commandDispatcher;
            this.accountValidator = accountValidator;
            this.accountTokenValidator = accountTokenValidator;
        }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Data { get; set; }

        [BindProperty]
        public ResetPasswordViewModel Form { get; set; }

        public Option<CommonError<AccountErrorType>> Error { get; private set; } = None;

        public async Task OnGet()
        {
            this.Form = new ResetPasswordViewModel(default, default);

            var contact = await this.accountTokenValidator.Validate(this.Token, this.Data)
                .BindAsync(this.accountValidator.Validate);

            this.Error = contact.Match(
                Right: _ => None,
                Left: Some);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var response = await this.SendCommand();
            return response.Match<IActionResult>(
                Right: _ => this.RedirectToPage("/Account/Login/Index"),
                Left: error =>
                {
                    this.Error = error;
                    return this.Page();
                });
        }

        private Task<Either<CommonError<AccountErrorType>, Unit>> SendCommand()
        {
            var command = new ResetPasswordCommand(this.Token, this.Data, this.Form.NewPassword);
            return this.commandDispatcher
                .Handle<ResetPasswordCommand, Task<Either<CommonError<AccountErrorType>, Unit>>>(command);
        }
    }
}
