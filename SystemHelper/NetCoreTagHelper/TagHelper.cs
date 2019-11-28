using System;
using System.Collections.Generic;
using System.Text;

namespace SystemHelper.NetCoreTagHelper
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System.Text;

    [HtmlTargetElement(Attributes = "asp-success-validation")]
    public class SuccessValidation : TagHelper
    {
        [HtmlAttributeName("success-message")]
        public string Message { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "text-success");
            output.TagMode = TagMode.StartTagAndEndTag;

            var stringOutput = "";

            if (!string.IsNullOrEmpty(this.Message) || !string.IsNullOrWhiteSpace(this.Message))
                stringOutput = $"{this.Message}";

            output.PreContent.SetHtmlContent(stringOutput);
        }
    }
}
