// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.ChangeAddress
{
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Nortridge.BorrowerPortal.Constants;
    using Nortridge.BorrowerPortal.Core;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Contacts;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Constants;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Exceptions;
    using Nortridge.Nls.XmlImportSerializer;
    using Nortridge.Nls.XmlImportSerializer.Services.Interfaces;
    using Nortridge.NlsWebApi.Client;

    public class IndexModel : PageModel
    {
        private readonly ContactService contactService;
        private readonly INlsXmlSerializationService nlsXmlSerializationService;
        private readonly ImportXmlService importXmlService;

        public IndexModel(
            ContactService contactService,
            INlsXmlSerializationService nlsXmlSerializationService,
            ImportXmlService importXmlService)
        {
            this.contactService = contactService;
            this.nlsXmlSerializationService = nlsXmlSerializationService;
            this.importXmlService = importXmlService;
        }

        [BindProperty]
        public ContactChangeAddressViewModel Form { get; set; }

        public bool RecordLocked { get; private set; }

        public bool Saved { get; private set; }

        public async Task OnGetAsync()
        {
            this.Saved = this.TempData[MessageConstants.ChangeOfAddress] as bool? ?? false;
            this.Form = await this.contactService.One(this.User.Id()).Map(Map);
        }

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
                    this.TempData[MessageConstants.ChangeOfAddress] = true;
                    return this.RedirectToPage();
                },
                Left: error =>
                {
                    if (error.Type == ChangeAddressErrorType.RecordLocked)
                    {
                        this.RecordLocked = true;
                    }

                    return this.Page();
                });
        }

        private static ContactChangeAddressViewModel Map(ContactPhoneNumbersVm contact) =>
            new ContactChangeAddressViewModel(
                contact.Street_Address1,
                contact.Street_Address2,
                contact.City,
                contact.State,
                contact.Zip,
                contact.County,
                contact.Country);

        private static NLSCIF MapContactXml(
            ContactPhoneNumbersVm contact,
            ContactChangeAddressViewModel contactAddress)
            =>
            new NLSCIF
            {
                CIFNumber = contact.Cifnumber,
                UpdateFlag = BooleanEnum.Item1,
                StreetAddress1 = contactAddress.Street1 ?? NlsApiConstants.ImportXmlEmptyValue,
                StreetAddress2 = contactAddress.Street2 ?? NlsApiConstants.ImportXmlEmptyValue,
                City = contactAddress.City ?? NlsApiConstants.ImportXmlEmptyValue,
                State = contactAddress.State ?? NlsApiConstants.ImportXmlEmptyValue,
                ZipCode = contactAddress.Zip ?? NlsApiConstants.ImportXmlEmptyValue,
                County = contactAddress.County ?? NlsApiConstants.ImportXmlEmptyValue,
                Country = contactAddress.Country ?? NlsApiConstants.ImportXmlEmptyValue
            };

        private static NLS MapRootXml(NLSCIF contact)
        {
            var root = new NLS
            {
                CommitBlock = BooleanEnum.Item1,
                EnforceTagExistence = BooleanEnum.Item1
            };

            root.CIF.Add(contact);

            return root;
        }

        private async Task<Either<CommonError<ChangeAddressErrorType>, Unit>> SendCommand()
        {
            var contact = await this.contactService.One(this.User.Id());
            var xml = this.Form
                .Apply(_ => MapContactXml(contact, _))
                .Apply(MapRootXml)
                .Apply(_ => this.nlsXmlSerializationService.SerializeXml(_));

            return await this.Import(xml);
        }

        private async Task<Either<CommonError<ChangeAddressErrorType>, Unit>> Import(string xml)
        {
            try
            {
                var result = await this.importXmlService.ImportXml(xml);
                return LanguageExt.Prelude.unit;
            }
            catch (NlsWebApiException ex) when (ex.StatusCode == 400)
            {
                var exResponse = ex.Response.Deserialize<NlsWebApiExceptionResponse>();
                if (exResponse.Status.Message != NlsWebApiExceptionStatus.ValidationErrorMessage)
                {
                    throw;
                }

                var responseError = exResponse.Errors[0];

                var result = responseError.IsTimeoutError() ?
                    new CommonError<ChangeAddressErrorType>(
                        ChangeAddressErrorType.RecordLocked,
                        string.Empty) :
                    new CommonError<ChangeAddressErrorType>(
                        ChangeAddressErrorType.Otherwise,
                        responseError.Message);

                return result;
            }
        }
    }
}
