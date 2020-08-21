// <copyright file="Error.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Exceptions;
    using Nortridge.NlsWebApi.Client;

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        private const int DefaultStatusCode = 100;
        private const int TaskCanceledStatusCode = 099;
        private const int ServerErrorStatusCode = 500;
        private readonly IStringLocalizer<ErrorModel> localizer;

        public ErrorModel(IStringLocalizer<ErrorModel> localizer)
        {
            this.localizer = localizer;
            this.Description = this.localizer["No description provided"];
            this.Status = DefaultStatusCode;
        }

        [BindProperty(SupportsGet = true)]
        public int? Status { get; set; }

        public string Description { get; private set; }

        public void OnPost()
        {
            var contextFeature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature == null)
            {
                return;
            }
            else if (contextFeature.Error is NlsWebApiException nlsApiException)
            {
                var ex = contextFeature.Error as NlsWebApiException;
                var exResponse = ex.Response.Deserialize<NlsWebApiExceptionResponse>();
                if (exResponse.Status.Message == NlsWebApiExceptionStatus.ValidationErrorMessage)
                {
                    this.Description = exResponse.Errors[0].Message;
                    this.Status = exResponse.Errors[0].Code;
                }
            }
            else if (contextFeature.Error is TaskCanceledException)
            {
                var ex = contextFeature.Error as TaskCanceledException;
                this.Status = TaskCanceledStatusCode;
                this.Description = ex.Message;
            }
            else
            {
                this.Status = ServerErrorStatusCode;
                this.Description = contextFeature.Error.Message;
            }
        }
    }
}
