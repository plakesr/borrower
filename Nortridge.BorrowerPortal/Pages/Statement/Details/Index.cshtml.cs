// <copyright file="Index.cshtml.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.Statement.Details
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Nortridge.BorrowerPortal.Core.Loans;

    public class IndexModel : PageModel
    {
        private readonly LoanService loanService;

        public IndexModel(LoanService loanService)
        {
            this.loanService = loanService;
        }

        [BindProperty(SupportsGet = true)]
        public long DocumentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var statement = await this.loanService.Statement(this.DocumentId);
            var binary = Convert.FromBase64String(statement.File);
            return new FileContentResult(binary, "application/pdf");
        }
    }
}
