
namespace KK.AspNetCore.Images.TagHelpers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;

    public class PictureTagOptions
    {
        public class Setting
        {
            public class Size
            {
                public string Name{ get; set; }
                public int Width { get; set; }
                public string Media { get; set; }
                public List<int> Zoom { get; set; } = new List<int>();
            }

            public string Name { get; set; }
            public List<Size> Sizes { get; set; } = new List<Size>();

            private string fallback;
            public string Fallback
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(fallback))
                    {
                        fallback = this.Sizes.First()?.Name ?? this.Sizes.First().Width.ToString();
                    }

                    return this.fallback;
                }
                set
                {
                    this.fallback = value;
                }
            }
        }

        private string imageFolder = "/images";
        public string ImageFolder
        {
            get { return this.imageFolder; }
            set
            {
                var newValue = value.Replace('\\', '/');
                if (!value.StartsWith("/", StringComparison.Ordinal))
                {
                    newValue = $"/{newValue}";
                }

                this.imageFolder = newValue;
            }
        }

        public List<Setting> Settings { get; set; } = new List<Setting>();
    }
}