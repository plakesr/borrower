// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Account.ChangePassword
{
    using System.Linq;
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Contacts;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Exceptions;
    using Nortridge.NlsWebApi.Client;

    public class IndexModel : PageModel
    {
        private readonly ContactService contactService;
        private readonly IStringLocalizer<IndexModel> localizer;

        public IndexModel(ContactService contactService, IStringLocalizer<IndexModel> localizer)
        {
            this.contactService = contactService;
            this.localizer = localizer;
        }

        [BindProperty]
        public ChangePasswordViewModel Form { get; set; }

        public void OnGet() =>
            this.Form = new ChangePasswordViewModel(
                currentPassword: default,
                newPassword: default,
                confirmPassword: default);

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var isChanged = await this.SendCommand();

            return isChanged ?
                (IActionResult)this.RedirectToPage() :
                this.Page();
        }

        private static bool IsCurrentUserPasswordError(NlsWebApiException ex)
        {
            var exResponse = ex.Response.Deserialize<NlsWebApiExceptionResponse>();
            var isCurrentUserPasswordError =
                exResponse.Status.Message == NlsWebApiExceptionStatus.ValidationErrorMessage &&
                exResponse.Errors.Any(_ => _.Path == NlsWebApiExceptionError.CurrentUserPasswordError);

            return isCurrentUserPasswordError;
        }

        private async Task<bool> SendCommand()
        {
            var model = new ContactWebCredentialsChangePasswordParam
            {
                Current_User_Password = this.Form.CurrentPassword,
                New_User_Password = this.Form.NewPassword
            };

            try
            {
                await this.contactService.ChangePassword(this.User.Id(), model);
                return true;
            }
            catch (NlsWebApiException ex) when (ex.StatusCode == 400 && IsCurrentUserPasswordError(ex))
            {
                this.ModelState.AddModelError(
                     key: $"{nameof(this.Form)}.{nameof(this.Form.CurrentPassword)}",
                     this.localizer["Invalid Current Password"]);
                return false;
            }
        }
    }
}
