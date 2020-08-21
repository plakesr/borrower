// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.SecuritySettings
{
    using System.Linq;
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Nortridge.BorrowerPortal.Core.Auth;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Contacts;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Exceptions;
    using Nortridge.NlsWebApi.Client;

    public class IndexModel : PageModel
    {
        // validation errors
        private const string IsUserNameInvalid = "User_Name";
        private const string Message = "Changing sequrity questions";

        private readonly ContactService contactService;
        private readonly IStringLocalizer<IndexModel> localizer;
        private readonly ILogger<AuthService> logger;

        public IndexModel(
            ContactService contactService,
            IStringLocalizer<IndexModel> localizer,
            ILogger<AuthService> logger)
        {
            this.contactService = contactService;
            this.localizer = localizer;
            this.logger = logger;
            this.Successful = false;
        }

        [BindProperty]
        public SecuritySettingsViewModel SecurityQuestions { get; set; }

        public bool RecordLocked { get; private set; }

        public bool Successful { get; private set; }

        public async Task OnGetAsync() =>
            this.SecurityQuestions = await this.Build();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return await this.RestorePage();
            }

            this.Successful = await this.SendCommand();

            return await this.RestorePage();
        }

        private async Task<IActionResult> RestorePage()
        {
            this.SecurityQuestions = await this.Build();
            return this.Page();
        }

        private async Task<SecuritySettingsViewModel> Build()
        {
            var credentials = await this.contactService.WebCredentials(this.User.Id());
            return Map(credentials.Data);
        }

        private static SecuritySettingsViewModel Map(ContactWebCredentialsVM credentials)
        {
            var questions1 = SecurityChallengeQuestion.Questions.ToSelectItems();
            var question1 = questions1.FirstOrDefault(_ => _.Text == credentials.Hint1);

            var q1 = question1 ?? questions1.First();
            q1.Selected = true;

            var questions2 = SecurityChallengeQuestion.Questions.ToSelectItems();
            var question2 = questions2.FirstOrDefault(_ => _.Text == credentials.Hint2);

            var q2 = question2 ?? questions2.First();
            q2.Selected = true;

            return new SecuritySettingsViewModel(questions1, questions2, credentials.Hint1_Answer, credentials.Hint2_Answer);
        }

        private async Task<bool> SendCommand()
        {
            var oldCredentials = await this.contactService.WebCredentials(this.User.Id());
            var model = new VersionedUpdateOfUpdateContactWebCredentialsParam()
            {
                Data = new UpdateContactWebCredentialsParam() {
                    Enable_Related_Loans = oldCredentials.Data.Enable_Related_Loans,
                    Web_Access_Enabled = oldCredentials.Data.Web_Access_Enabled,
                    User_Name = oldCredentials.Data.User_Name,
                    Hint1 = this.SecurityQuestions.Question1,
                    Hint1_Answer = this.SecurityQuestions.Answer1,
                    Hint2 = this.SecurityQuestions.Question2,
                    Hint2_Answer = this.SecurityQuestions.Answer2,
                },
                VersionHash = oldCredentials.VersionHash,
            };

            try
            {
                await this.contactService.UpdateWebCredentials(this.User.Id(), model);
                return true;
            }
            catch (NlsWebApiException ex) when (ex.StatusCode == 400)
            {
                var exResponse = ex.Response.Deserialize<NlsWebApiExceptionResponse>();
                if (exResponse.Status.Message != NlsWebApiExceptionStatus.ValidationErrorMessage)
                {
                    throw;
                }

                var responseError = exResponse.Errors[0];

                if (responseError.IsTimeoutError())
                {
                    this.ModelState.AddModelError(
                         key: string.Empty,
                         this.localizer["Record locked. Try again later"]);
                }
                else
                {
                    if (exResponse.Errors.Any(_ => _.Path == IsUserNameInvalid))
                    {
                        this.ModelState.AddModelError(
                             key: string.Empty,
                             this.localizer["WebCredentials username error. Call support, please."]);
                    }
                    else
                    {
                        this.ModelState.AddModelError(
                             key: string.Empty,
                             this.localizer["Unknown error. Call support, please."]);
                        this.logger.LogError(message: Message, exResponse.Serialize());
                    }
                }
                return false;
            }
        }
    }
}
