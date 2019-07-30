
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

        private readonly string sourceFolder = Path.DirectorySeparatorChar + "Images";
        public string SourceFolder
        {
            get => this.sourceFolder;
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

        private string targetFolder = "/images/generated";
        public string TargetFolder
        {
            get => this.targetFolder;
            set
            {
                var newValue = value.Replace('\\', '/');

                if (!value.StartsWith("/", StringComparison.Ordinal))
                {
                    newValue = $"/{newValue}";
                }
                if (newValue == "/")
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(this.TargetFolder),
                        value,
                        "Root not allowed. You must set a valid folder."
                    );
                }

                this.targetFolder = newValue;
            }
        }

        public class OutputFormat
        {
            public string[] FileEndings { get; set; }
        }

        public List<OutputFormat> OutputFormats { get; set; } = new List<OutputFormat>();
        public bool LosslessCompress { get; set; } = false;

        public bool WriteMetaFiles { get; set; } = false;
    }
}
