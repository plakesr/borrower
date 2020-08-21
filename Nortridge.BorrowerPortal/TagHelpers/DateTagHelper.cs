// <copyright file="DateTagHelper.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.TagHelpers
{
    using System;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Nortridge.BorrowerPortal.Core.Extensions;

    public class DateTagHelper : TagHelper
    {
        private readonly IHtmlGenerator htmlGenerator;

        public DateTagHelper(IHtmlGenerator htmlGenerator)
        {
            this.htmlGenerator = htmlGenerator;
        }

        public ModelExpression For { get; set; }

        public bool Disabled { get; set; }

        public DateTime? Min { get; set; }

        public DateTime? Max { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            var id = TagBuilder.CreateSanitizedId(this.For.Name, this.htmlGenerator.IdAttributeDotReplacement);
            var dateId = id + "-date";

            output.Attributes.Add(new TagHelperAttribute("class", "input-group date js-date-picker"));
            output.Attributes.Add(new TagHelperAttribute("data-target-input", "nearest"));
            output.Attributes.Add(new TagHelperAttribute("id", dateId));

            if (this.Min.HasValue)
            {
                output.Attributes.Add(new TagHelperAttribute("data-min-date", this.Min.Value.ToDatePickerISODate()));
            }

            if (this.Max.HasValue)
            {
                output.Attributes.Add(new TagHelperAttribute("data-max-date", this.Max.Value.ToDatePickerISODate()));
            }

            var input = this.CreateInput(id, dateId);
            output.Content.AppendHtml(input);

            var html = $@"
                <div class=""input-group-append"" data-target=""#{dateId}""
                     data-toggle=""datetimepicker"">
                    <div class=""input-group-text""></div>
                </div>";

            output.PostContent.AppendHtml(html);
        }

        private static DateTime? GetValue(object model) =>
            model == null ? null : (DateTime?)model;

        private TagBuilder CreateInput(string id, string dateId)
        {
            var input = this.htmlGenerator.GenerateTextBox(
                viewContext: this.ViewContext,
                modelExplorer: this.For.ModelExplorer,
                expression: this.For.Name,
                value: GetValue(this.For.Model).ToDatePickerDate(),
                format: null,
                htmlAttributes: new
                {
                    type = "text",
                    @class = "form-control form-control--custom form-control--custom__data datetimepicker-input js-date-input",
                    id,
                    data_target = "#" + dateId,
                    data_val = "true",
                    data_val_date = string.Empty
                });

            if (this.Disabled)
            {
                input.MergeAttribute("disabled", "disabled");
            }

            return input;
        }
    }
}
