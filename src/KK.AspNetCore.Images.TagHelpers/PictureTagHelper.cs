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
        private const string ClassAttributeName = "class";
        private const string StyleAttributeName = "style";
        private const string SettingAttributeName = "setting";

        [HtmlAttributeName(SourceAttributeName)]
        public string Src { get; set; }

        [HtmlAttributeName(AltAttributeName)]
        public string Alt { get; set; }

        [HtmlAttributeName(ClassAttributeName)]
        public string CssClass { get; set; }

        [HtmlAttributeName(StyleAttributeName)]
        public string Style { get; set; }

        [HtmlAttributeName(SettingAttributeName)]
        public string Setting { get; set; } = "default";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = new StringBuilder();
            var src = context.AllAttributes["src"].Value.ToString();
            var altText = context.AllAttributes["alt"]?.Value.ToString();
            var styleAttrib = !String.IsNullOrWhiteSpace(this.Style) ? $" style=\"{this.Style}\"" : "";
            var classAttrib = !String.IsNullOrWhiteSpace(this.CssClass) ? $" class=\"{this.CssClass}\"" : "";
            var setting = this.options.Settings.SingleOrDefault(s => s.Name == this.Setting);

            foreach (var size in setting?.Sizes)
            {
                var mediaAttribute = !String.IsNullOrWhiteSpace(size.Media) ? $" media=\"{size.Media}\"" : "";
                content.AppendLine($"<source srcset=\"{GetSourceAttribute(src, size)}\"{mediaAttribute}>");
            }

            var fallbackSize = setting.Sizes.Single(
                 s =>
                 {
                     return s.Name == setting.Fallback || ( string.IsNullOrWhiteSpace(s.Name) && s.Width.ToString() == setting.Fallback);
                 });
            var imgSrcSetAttribute = string.Empty;
            if (fallbackSize?.Zoom.Count > 0)
            {
                imgSrcSetAttribute = $" srcset=\"{GetSourceAttribute(src, fallbackSize)}\"";
            }

            content.AppendLine($"<img{classAttrib}{styleAttrib} src=\"{this.options.ImageFolder}/{src}/{setting.Fallback}.jpg\"{imgSrcSetAttribute} alt=\"{altText}\">");

            output.TagName = "picture";
            output.Attributes.RemoveAll("src");
            output.Attributes.RemoveAll("alt");
            output.Attributes.RemoveAll("class");
            output.Attributes.RemoveAll("style");
            output.Attributes.RemoveAll("setting");
            output.Content.SetHtmlContent(content.ToString());
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        private string GetSourceAttribute(string src, PictureTagOptions.Setting.Size size)
        {
            var filename = !string.IsNullOrWhiteSpace(size.Name) ? size.Name : size.Width.ToString();
            var sourceAttributeText = $"{this.options.ImageFolder}/{src}/{filename}.jpg";
            foreach (var zoom in size.Zoom.Where(x => x > 1).Distinct())
            {
                var zoomfilename = !string.IsNullOrWhiteSpace(size.Name) ? $"{size.Name}{zoom}x" : (size.Width * zoom).ToString();
                sourceAttributeText += $", {this.options.ImageFolder}/{src}/{zoomfilename}.jpg {zoom}x";
            }

            return sourceAttributeText;
        }
    }
}
