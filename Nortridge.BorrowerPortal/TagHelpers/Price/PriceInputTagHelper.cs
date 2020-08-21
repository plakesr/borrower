// <copyright file="PriceInputTagHelper.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.TagHelpers.Price
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Nortridge.BorrowerPortal.Core.Extensions;

    [HtmlTargetElement("input", Attributes = "price-input")]
    public class PriceInputTagHelper : TagHelper
    {
        public bool Optional { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var rawValue = output.Attributes["value"].Value.ToString();
            var price = this.Price(rawValue);

            output.Attributes.Add("class", "form-control form-control--custom text-right js-price-input");
            output.Attributes.Add(new TagHelperAttribute("title", price));

            output.Attributes.SetAttribute("type", "text");
            output.Attributes.SetAttribute("value", price.ToShortStringPrice());
        }

        private decimal? Price(string source) =>
            source.HasValue() ? source.ToDecimal() : (this.Optional ? default(decimal?) : 0);
    }
}
