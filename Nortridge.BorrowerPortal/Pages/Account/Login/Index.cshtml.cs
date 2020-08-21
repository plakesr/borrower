// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account
{
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Nortridge.BorrowerPortal.Core.Auth;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Constants;

    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly AuthService authService;
        private readonly IStringLocalizer<IndexModel> localizer;
        private readonly ILogger<IndexModel> logger;

        public IndexModel(
            AuthService authService,
            IStringLocalizer<IndexModel> localizer,
            ILogger<IndexModel> logger)
        {
            this.authService = authService;
            this.localizer = localizer;
            this.logger = logger;
        }

        public bool UserRegistered { get; private set; }

        public bool ForgotPasswordSent { get; private set; }

        public LoginViewModel FormLogin { get; private set; }

        public SecurityQuestionViewModel FormQuestion { get; private set; }

        public void OnGet()
        {
            this.FormLogin = new LoginViewModel(default, default);
            this.UserRegistered = this.TempData[AccountConstants.UserRegistered] as bool? ?? false;
            this.ForgotPasswordSent = this.TempData[AccountConstants.ForgotPasswordSent] as bool? ?? false;
        }

        public async Task<IActionResult> OnPostAnswerAsync(SecurityQuestionViewModel formQuestion, string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var formLogin = ((string)this.TempData[formQuestion.Username]).FromJson<LoginViewModel>();

            var signInResult = await this.authService.SignIn(formLogin, formQuestion);
            return signInResult.Match(
                Right: _ =>
                {
                    this.logger.LogInformation("The user {0} is logged in.", formLogin.Username);
                    return this.GetRedirectResult(returnUrl);
                },
                Left: error =>
                {
                    var errorMessage = this.ErrorMessage(error.AuthError);
                    this.ModelState.AddModelError(key: string.Empty, errorMessage);
                    this.FormLogin = new LoginViewModel(formLogin.Username, string.Empty);
                    return this.Page();
                });
        }

        public async Task<IActionResult> OnPostAsync(LoginViewModel formLogin, string returnUrl = null)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var signInResult = await this.authService.SignIn(formLogin);
            return signInResult.Match(
                Right: _ =>
                {
                    this.logger.LogInformation("The user {0} is logged in.", formLogin.Username);
                    return this.GetRedirectResult(returnUrl);
                },
                Left: error =>
                {
                    if (error.AuthError == AuthError.ShowSecurityQuestion)
                    {
                        this.FormQuestion = new SecurityQuestionViewModel(formLogin.Username, error.QuestionData.Question, error.QuestionData.IntHint);
                        this.TempData[formLogin.Username] = formLogin.ToJson();
                        this.TempData.Keep();
                        return this.Page();
                    }

                    var errorMessage = this.ErrorMessage(error.AuthError);
                    this.ModelState.AddModelError(key: string.Empty, errorMessage);
                    this.FormQuestion = null;
                    return this.Page();
                });
        }

        private IActionResult GetRedirectResult(string returnPath)
        {
            if (!returnPath.HasValue())
            {
                return this.RedirectToPage("/Index");
            }

            var logoutPath = this.Url.Page("/Account/Logout");
            var isLogout = returnPath == logoutPath;

            return isLogout ?
                (IActionResult)this.RedirectToPage("/Index") :
                this.LocalRedirect(returnPath);
        }

        private LocalizedString ErrorMessage(AuthError error)
        {
            switch (error)
            {
                case AuthError.Otherwise:
                    return this.localizer["Username, password or security question answer was not valid. Please try again."];
                case AuthError.AccountLocked:
                    return this.localizer["Account is locked. Attempted more than 5x in a day."];
                case AuthError.NoLoans:
                    return this.localizer["This user has no active loans."];
                default:
                    throw new InvalidEnumArgumentException(nameof(error), (int)error, typeof(AuthError));
            }
        }
    }
}
