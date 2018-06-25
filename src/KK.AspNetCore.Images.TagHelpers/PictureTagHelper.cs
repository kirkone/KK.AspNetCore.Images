namespace KK.AspNetCore.Images.TagHelpers
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("picture", Attributes = SourceAttributeName)]
    public class PictureTagHelper : TagHelper
    {
        private readonly PictureTagOptions options;

        public PictureTagHelper(
            PictureTagOptions options
        )
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.options = options;
        }

        private const string SourceAttributeName = "src";
        private const string AltAttributeName = "alt";
        private const string StyleAttributeName = "style";
        private const string SettingAttributeName = "setting";

        [HtmlAttributeName(SourceAttributeName)]
        public string Src { get; set; }
        [HtmlAttributeName(AltAttributeName)]
        public string Alt { get; set; }
        [HtmlAttributeName(StyleAttributeName)]
        public string Style { get; set; }
        [HtmlAttributeName(SettingAttributeName)]
        public string Setting { get; set; } = "default";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = new StringBuilder();
            var src = context.AllAttributes["src"].Value;
            var altText = context.AllAttributes["alt"]?.Value;
            var styleAttrib = !String.IsNullOrWhiteSpace(this.Style) ? $" style=\"{this.Style}\"" : "";

            foreach (var source in this.options.Settings.SingleOrDefault( s => s.Name == this.Setting)?.Sizes)
            {
                var mediaAttrib = !String.IsNullOrWhiteSpace(source.Media) ? $" media=\"{source.Media}\"" : "";
                content.AppendLine($"<source srcset=\"{this.options.ImageFolder}/{src}/{source.Name}.jpg\"{mediaAttrib}>");
            }
            content.AppendLine($"<img{styleAttrib} src=\"{src}\" alt=\"{altText}\">");

            output.TagName = "picture";
            output.Attributes.RemoveAll("alt");
            output.Attributes.RemoveAll("src");
            output.Attributes.RemoveAll("style");
            output.Attributes.RemoveAll("setting");
            output.Content.SetHtmlContent(content.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
