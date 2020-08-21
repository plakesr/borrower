// <copyright file="PricelongTagHelper.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.TagHelpers.Price
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Nortridge.BorrowerPortal.Core.Extensions;

    public class PricelongTagHelper : TagHelper
    {
        public double? Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            if (!this.Value.HasValue)
            {
                return;
            }

            output.Attributes.Add(new TagHelperAttribute("title", this.Value));
            output.Content.SetContent(this.Value.Value.ToLongStringPrice());
        }
    }
}
