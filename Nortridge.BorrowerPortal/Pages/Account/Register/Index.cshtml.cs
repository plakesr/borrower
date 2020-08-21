// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account.Register
{
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Auth;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Commands;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Constants;
    using Nortridge.BorrowerPortal.Core.UseCases.Account;

    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly IStringLocalizer<IndexModel> localizer;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly AuthService authService;

        public IndexModel(
            IStringLocalizer<IndexModel> localizer,
            ICommandDispatcher commandDispatcher,
            AuthService authService)
        {
            this.localizer = localizer;
            this.commandDispatcher = commandDispatcher;
            this.authService = authService;
        }

        [BindProperty]
        public RegisterViewModel Form { get; set; }

        public bool RecordLocked { get; private set; }

        public void OnGet() => this.Form = Build();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.RestoreState();
            }

            var response = await this.SendCommand();
            return response.Match(
                Right: _ => this.SuccessResult(),
                Left: this.ErrorResult);
        }

        private static RegisterViewModel Build()
        {
            var questions = SecurityChallengeQuestion.Questions.ToSelectItems();

            return new RegisterViewModel(
                username: default,
                newPassword: default,
                confirmPassword: default,
                securityQuestion1List: questions,
                securityQuestion1: default,
                securityQuestion1Answer: default,
                securityQuestion2List: questions,
                securityQuestion2: default,
                securityQuestion2Answer: default,
                firstName: default,
                lastName: default,
                email: default,
                dateOfBirth: default,
                alwaysTrustThisComputer: default);
        }

        private IActionResult SuccessResult()
        {
            this.TempData[AccountConstants.UserRegistered] = true;
            this.TempData.Keep();

            if (this.Form.AlwaysTrustThisComputer)
            {
                this.authService.SetTrustCookie();
            }

            return this.RedirectToPage("/Account/Login/Index");
        }

        private IActionResult ErrorResult(CommonError<RegisterErrorType> error)
        {
            switch (error.Type)
            {
                case RegisterErrorType.UserNameNotUnique:
                    this.ModelState.AddModelError(key: string.Empty, this.localizer["Unable to create credentials: User Name not unique"]);
                    this.ModelState.AddModelError(key: $"{nameof(this.Form)}.{nameof(this.Form.Username)}", this.localizer["UserName not unique"]);
                    break;
                case RegisterErrorType.UnknownUser:
                case RegisterErrorType.UnableToMatchUser:
                    this.ModelState.AddModelError(key: string.Empty, this.localizer["Unable to validate user: Unable to Verify Contact"]);
                    break;
                case RegisterErrorType.UserNameTooShort:
                    this.ModelState.AddModelError(key: string.Empty, this.localizer["Unable to create credentials: User Name Too Short"]);
                    this.ModelState.AddModelError(key: $"{nameof(this.Form)}.{nameof(this.Form.Username)}", this.localizer["User Name Too Short"]);
                    break;
                case RegisterErrorType.RecordLocked:
                    this.RecordLocked = true;
                    break;
            }

            return this.RestoreState();
        }

        private IActionResult RestoreState()
        {
            this.Form = Build();
            return this.Page();
        }

        private Task<Either<CommonError<RegisterErrorType>, Unit>> SendCommand()
        {
            var command = new RegisterCommand(this.Form);
            return this.commandDispatcher
                .Handle<RegisterCommand, Task<Either<CommonError<RegisterErrorType>, Unit>>>(command);
        }
    }
}
