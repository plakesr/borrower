namespace Nortridge.BorrowerPortal.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("input", Attributes = "submit")]
    [HtmlTargetElement("select", Attributes = "submit")]
    public class SubmitTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll("submit");
            output.Attributes.Add("onchange", "this.form.submit()");
        }
    }
}
