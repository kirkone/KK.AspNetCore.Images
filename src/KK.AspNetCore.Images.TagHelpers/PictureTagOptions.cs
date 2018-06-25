
namespace KK.AspNetCore.Images.TagHelpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PictureTagOptions
    {
        public class Setting
        {
            public class Size
            {
                public string Name { get; set; }
                public string Media { get; set; }
            }

            public string Name { get; set; }
            public List<Size> Sizes { get; set; } = new List<Size>();
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
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