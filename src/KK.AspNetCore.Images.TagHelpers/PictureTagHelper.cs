using System;

namespace KK.AspNetCore.Images.TagHelpers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("picture", Attributes = SourceAttributeName)]
    public class PictureTagHelper : TagHelper
    {
        private const string SourceAttributeName = "src";
        private const string AltAttributeName = "alt";
        private const string StyleAttributeName = "style";

        [HtmlAttributeName(SourceAttributeName)]
        public string Src { get; set; }
        [HtmlAttributeName(AltAttributeName)]
        public string Alt { get; set; }
        [HtmlAttributeName(StyleAttributeName)]
        public string Style { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = new StringBuilder();
            var src = context.AllAttributes["src"].Value;
            var altText = context.AllAttributes["alt"]?.Value;
            var styleAtrib = !String.IsNullOrWhiteSpace(this.Style) ? $" style=\"{this.Style}\"" : "";

            content.AppendLine($"<img{styleAtrib} src=\"{src}\" alt=\"{altText}\">");

            output.TagName = "picture";
            output.Attributes.RemoveAll("alt");
            output.Attributes.RemoveAll("src");
            output.Attributes.RemoveAll("style");
            output.Content.SetHtmlContent(content.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
