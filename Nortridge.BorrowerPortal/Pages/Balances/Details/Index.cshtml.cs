// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Balances.Details
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Localization;
    using Nortridge.BorrowerPortal.Core.Extensions;
    using Nortridge.BorrowerPortal.Core.Loans;
    using Nortridge.BorrowerPortal.Infrastructure;
    using Nortridge.BorrowerPortal.Infrastructure.Breadcrumbs;
    using Nortridge.NlsWebApi.Client;

    public class IndexModel : PageModel
    {
        private readonly IStringLocalizer<IndexModel> localizer;
        private readonly LoanService loanService;
        private readonly LoanSettingsService loanSettingsService;

        public IndexModel(
            IStringLocalizer<IndexModel> localizer,
            LoanService loanService,
            LoanSettingsService loanSettingsService)
        {
            this.localizer = localizer;
            this.loanService = loanService;
            this.loanSettingsService = loanSettingsService;
        }

        [BindProperty(SupportsGet = true)]
        public long LoanId { get; set; }

        public LoanBalanceDetailsViewModel Loan { get; private set; }

        public BreadcrumbModel CurrentLevel { get; private set; }

        public async Task OnGetAsync()
        {
            var loan = await this.loanService.One(this.LoanId);
            var groups = await this.loanSettingsService.Groups();

            this.Loan = Map(
                loan,
                this.loanService.GroupCode(groups, loan.Loan_Group_No),
                await this.loanService.PaymentInfo(this.LoanId),
                await this.loanService.BillingInfo(this.LoanId));

            this.CurrentLevel = new BreadcrumbModel(
                $"{this.localizer["Loan Balance Details"]} {loan.Loan_Number}",
                this.RouteData.CurrentPage());
        }

        private static LoanBalanceDetailsViewModel Map(
            LoanDto loan,
            string groupCode,
            Loanacct_PaymentDto paymentInfo,
            BillingInfoVM billingInfo)
            =>
            new LoanBalanceDetailsViewModel(
                number: loan.Loan_Number,
                nickname: loan.Borrower_Loan_Nickname,
                openDate: loan.Open_Date,
                maturityDate: loan.Curr_Maturity_Date,
                loanAmount: loan.Current_Note_Amount,
                balanceDate: loan.Interest_Accrued_Thru_Date,
                principal: loan.Current_Principal_Balance,
                interest: loan.Current_Interest_Balance,
                fees: loan.Current_Fees_Balance,
                lateCharges: loan.Current_Late_Charge_Balance,
                otherCharges: paymentInfo.Current_Pending,
                totalBalance: loan.Current_Payoff_Balance,
                currentImpound: loan.Current_Impound_Balance,
                interestAccruedThrough: loan.Interest_Accrued_Thru_Date,
                perdiemInterest: loan.Current_Perdiem,
                nextPrincipalBilling: paymentInfo.Next_Principal_Due_Amount,
                nextPrincipalBillingDueDate: paymentInfo.Next_Principal_Payment_Date,
                nextInterestBilling: paymentInfo.Next_Interest_Due_Amount,
                nextInterestBillingDueDate: paymentInfo.Next_Interest_Payment_Date,
                nextTotalPayment: paymentInfo.Next_Payment_Total_Amount,
                nextTotalBilling: NextTotalBilling(paymentInfo, billingInfo),
                lastPayment: paymentInfo.Last_Payment_Amount,
                lastPaymentReceivedDate: paymentInfo.Last_Payment_Date,
                totalPastDue: loan.Total_Past_Due_Balance,
                totalCurrentDue: loan.Total_Current_Due_Balance,
                loanType: groupCode,
                currentRate: loan.Current_Interest_Rate,
                daysPastDue: loan.Days_Past_Due);

        private static double NextTotalBilling(
            Loanacct_PaymentDto paymentInfo,
            BillingInfoVM billingInfo)
            =>
            billingInfo.Next_Recurring_Billing + billingInfo.Next_Impound_Billing +
                paymentInfo.Next_Payment_Total_Amount;
    }
}
