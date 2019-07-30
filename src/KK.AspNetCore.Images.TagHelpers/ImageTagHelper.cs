namespace KK.AspNetCore.Images.TagHelpers
{
    using System;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("image", Attributes = SourceAttributeName)]
    public class ImageTagHelper : TagHelper
    {
        private readonly ImageTagOptions options;

        public ImageTagHelper(
            ImageTagOptions options
        ) => this.options = options ?? throw new ArgumentNullException(nameof(options));

        private const string SourceAttributeName = "src";
        private const string SizesAttributeName = "sizes";
        private const string AltAttributeName = "alt";
        private const string ClassAttributeName = "class";
        private const string StyleAttributeName = "style";
        private const string SettingAttributeName = "setting";

        [HtmlAttributeName(SourceAttributeName)]
        public string Src { get; set; }

        [HtmlAttributeName(SizesAttributeName)]
        public string Sizes { get; set; }

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
            var srcset = new StringBuilder();
            var setting = this.options.Settings.SingleOrDefault(s => s.Name == this.Setting);

            foreach (var size in setting?.Sizes)
            {
                _ = srcset.Append($"{this.GetSourceAttribute(this.Src, size)} {size.Width}w,");
            }

            var fallbackSize = setting.Sizes.Single(
                 s => s.Name == setting.Fallback || (string.IsNullOrWhiteSpace(s.Name) && s.Width.ToString() == setting.Fallback));

            output.TagName = "img";

            output.Attributes.Add("src", this.GetSourceAttribute(this.Src, fallbackSize));
            output.Attributes.Add("srcset", srcset);
            if (!string.IsNullOrWhiteSpace(this.Style))
            {
                output.Attributes.Add("style", this.Style);
            }
            if (!string.IsNullOrWhiteSpace(this.CssClass))
            {
                output.Attributes.Add("class", this.CssClass);
            }
            if (!string.IsNullOrWhiteSpace(this.Alt))
            {
                output.Attributes.Add("alt", this.Alt);
            }
            if (!string.IsNullOrWhiteSpace(this.Sizes))
            {
                output.Attributes.Add("sizes", this.Sizes);
            }
            output.TagMode = TagMode.SelfClosing;
        }

        private string GetSourceAttribute(string src, ImageTagOptions.Setting.Size size)
        {
            var filename = !string.IsNullOrWhiteSpace(size.Name) ? size.Name : size.Width.ToString();
            var sourceAttributeText = $"{this.options.ImageFolder}/{src}/{filename}.jpg";

            return sourceAttributeText;
        }
    }
}
