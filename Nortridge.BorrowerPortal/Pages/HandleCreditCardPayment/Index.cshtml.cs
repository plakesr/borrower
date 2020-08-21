// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.HandleCreditCardPayment
{
    using System.Globalization;
    using System.Threading.Tasks;
    using LanguageExt;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.BorrowerPortal.Core.Infrastructure.Commands;
    using Nortridge.NlsWebApi.Client;
    using static LanguageExt.Prelude;

    [AllowAnonymous]
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly WebPayService webPayService;
        private readonly ILogger<IndexModel> logger;
        private readonly ISystemClock clock;
        private readonly ICommandDispatcher commandDispatcher;

        public IndexModel(
            WebPayService webPayService,
            ILogger<IndexModel> logger,
            ISystemClock clock,
            ICommandDispatcher commandDispatcher)
        {
            this.webPayService = webPayService;
            this.logger = logger;
            this.clock = clock;
            this.commandDispatcher = commandDispatcher;
        }

        private string Confirmation { get; set; }

        public async Task<IActionResult> OnPostAsync(PaymentPostData postData)
        {
            var log = "The payment information from merchant was handled " + this.clock.UtcNow.ToString("F", CultureInfo.CreateSpecificCulture("en-US"));

            this.logger.LogInformation(log);

            await this.SendCommand(postData);

            return this.StatusCode(200);
        }

        private async Task<Unit> SendCommand(PaymentPostData postData)
        {
            var model = new PostWebPayParam()
            {
                AccountID = postData.AcctId,
                AccountSubID = postData.SubId,
                Confirmation = this.Confirmation,
                Loan_Number_Encrypted = postData.Customdata,
                OrderID = postData.OrderId,
                Payment_Amount = postData.Amount,
                Status = postData.Status,
                TransactionID = postData.HistoryId,
            };

            await this.webPayService.PostWebPay(model);
            return unit;
        }
    }
}
