
namespace KK.AspNetCore.Images.Processing
{
    using System;
    using System.IO;

    public class ImageProcessingMiddlewareOptions
    {
        private string sourceFolder = System.IO.Path.DirectorySeparatorChar + "Images";
        public string SourceFolder
        {
            get { return this.sourceFolder; }
            set
            {
                var newValue = value.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
                if (!value.StartsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
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

    }
}