// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Nicknames
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Constants;
    using Nortridge.BorrowerPortal.Core.Auth.Identity;
    using Nortridge.BorrowerPortal.Core.Contacts;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Infrastructure;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Exceptions;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.NlsWebApi.Client;
    using static LanguageExt.Prelude;

    public class IndexModel : PageModel
    {
        private readonly ContactService contactService;
        private readonly LoanService loanService;
        private readonly LoanSettingsService loanSettingsService;
        private readonly IStringLocalizer<IndexModel> localizer;
        private const string IsNickNameInvalid = "Nick_Name";

        public IndexModel(
            ContactService contactService,
            LoanService loanService,
            LoanSettingsService loanSettingsService,
            IStringLocalizer<IndexModel> localizer)
        {
            this.contactService = contactService;
            this.loanService = loanService;
            this.loanSettingsService = loanSettingsService;
            this.localizer = localizer;
        }

        [BindProperty]
        public SecuritySettingsViewModel[] Loans { get; set; }

        public bool RecordLocked { get; private set; }

        public bool Saved { get; private set; }

        public async Task OnGetAsync()
        {
            this.Saved = this.TempData[MessageConstants.Nicknames] as bool? ?? false;
            this.Loans = await this.Build();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!this.ModelState.IsValid)
            {
                return await this.RestorePage();
            }

            var response = await this.SendCommand();
            return await response.Match(
                Right: _ =>
                {
                    this.TempData[MessageConstants.Nicknames] = true;
                    return this.RedirectToPage().Apply(Task.FromResult<IActionResult>);
                },
                Left: error =>
                {
                    if (error.Type == NicknamesErrorType.RecordLocked)
                    {
                        this.RecordLocked = true;
                    }

                    return this.RestorePage();
                });
        }

        private static SecuritySettingsViewModel Map(LoanDto loan, string groupCode) =>
            new SecuritySettingsViewModel(
                loanId: loan.Acctrefno,
                loanNumber: loan.Loan_Number,
                accountDescription: groupCode,
                nickname: loan.Borrower_Loan_Nickname);

        private SecuritySettingsViewModel Map(
            ICollection<LoanGroupVM> groups,
            LoanDto loan)
        {
            var groupCode = this.loanService.GroupCode(groups, loan.Loan_Group_No);
            return Map(loan, groupCode);
        }

        private async Task<IActionResult> RestorePage()
        {
            this.Loans = await this.Build();
            return this.Page();
        }

        private async Task<SecuritySettingsViewModel[]> Build()
        {
            var loans = await this.contactService.Loans(this.User.Id());
            var groups = await this.loanSettingsService.Groups();

            return loans.Select(_ => this.Map(groups, _)).ToArray();
        }

        private static (UpdateBorrowerLoanNicknameParam param, SecuritySettingsViewModel loan) ToParam(SecuritySettingsViewModel loan)
        {
            var param = new UpdateBorrowerLoanNicknameParam
            {
                Borrower_Loan_Nickname = loan.Nickname
            };

            return (param, loan);
        }

        private static bool IsNicknameError(NlsWebApiException ex)
        {
            var exResponse = ex.Response.Deserialize<NlsWebApiExceptionResponse>();
            var isNicknameError =
                exResponse.Status.Message == NlsWebApiExceptionStatus.ValidationErrorMessage &&
                exResponse.Errors.Any(_ => _.Path == NlsWebApiExceptionError.NicknameIsntValidError);

            return isNicknameError;
        }

        private static bool IsChanged(SecuritySettingsViewModel newLoan, LoanDto oldLoan) =>
            newLoan.Nickname == null ?
                false :
                !newLoan.Nickname.Equals(oldLoan.Borrower_Loan_Nickname, StringComparison.Ordinal);

        private async Task<Either<CommonError<NicknamesErrorType>, Unit>> SendCommand()
        {
            var oldLoans = await this.contactService.Loans(this.User.Id());
            var changedLoans = this.Loans
                .Join(
                    oldLoans,
                    _ => _.LoanId,
                    _ => _.Acctrefno,
                    (newLoan, oldLoan) => (newLoan, isChanged: IsChanged(newLoan, oldLoan)))
                .Where(_ => _.isChanged)
                .Select(_ => ToParam(_.newLoan));

            try
            {
                await changedLoans
                    .Select(_ => this.loanService.UpdateBorrowerLoanNickname(_.loan.LoanId, _.param))
                    .Apply(Task.WhenAll);
                return unit;
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
                    return new CommonError<NicknamesErrorType>(
                        NicknamesErrorType.RecordLocked,
                        this.localizer["The record locked"]);
                }
                else
                {
                    if (exResponse.Errors.Any(_ => _.Path == IsNickNameInvalid))
                    {
                        return new CommonError<NicknamesErrorType>(
                        NicknamesErrorType.RecordLocked,
                        this.localizer["Nickname is invalid."]);
                    }
                }

                return new CommonError<NicknamesErrorType>(
                    NicknamesErrorType.RecordLocked,
                    this.localizer["Unknown error. Call support, please."]);
            }
        }
    }
}
