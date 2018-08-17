
namespace KK.AspNetCore.Images.Processing
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ImageProcessingOptions
    {
        public class Size
        {
            public string Name { get; set; }
            public int Width { get; set; } = 0;
            public int Height { get; set; } = 0;
            public int Quality { get; set; } = 0;
            public bool Progressive { get; set; } = true;
        }

        public List<Size> Sizes { get; set; } = new List<Size>();

        private string sourceFolder = System.IO.Path.DirectorySeparatorChar + "Images";
        public string SourceFolder
        {
            get { return this.sourceFolder; }
            set
            {
                var newValue = value.Replace(
                        '\\',
                        Path.DirectorySeparatorChar
                    ).Replace(
                        '/',
                        Path.DirectorySeparatorChar
                    );

                if (
                    !value.StartsWith(
                        Path.DirectorySeparatorChar.ToString(),
                        StringComparison.Ordinal
                    )
                )
                {
                    newValue = $"{Path.DirectorySeparatorChar}{newValue}";
                }

                this.targetFolder = newValue;
            }
        }

        private string targetFolder = "/images";
        public string TargetFolder
        {
            get { return this.targetFolder; }
            set
            {
                var newValue = value.Replace('\\', '/');
                if (!value.StartsWith("/", StringComparison.Ordinal))
                {
                    newValue = $"/{newValue}";
                }

                this.targetFolder = newValue;
            }
        }

        public bool LosslessCompress = false;
    }
}