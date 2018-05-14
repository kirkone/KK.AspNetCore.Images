namespace KK.AspNetCore.Images.Processing
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using ImageMagick;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Logging;

    public class ImageProcessingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ImageProcessingMiddlewareOptions options;
        private readonly ILogger<ImageProcessingMiddleware> logger;
        private readonly IHostingEnvironment env;

        public ImageProcessingMiddleware(
            RequestDelegate next,
            ImageProcessingMiddlewareOptions options,
            IHostingEnvironment env,
            ILogger<ImageProcessingMiddleware> logger
        )
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (env == null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.next = next;
            this.options = options;
            this.env = env;
            this.logger = logger;
        }

        /// <summary>
        /// Processes a request to determine if it matches a known image, otherwise it will generate it.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;

            if (!Helpers.IsGetOrHeadMethod(context.Request.Method))
            {
                this.logger.LogRequestMethodNotSupported(context.Request.Method);
            }
            else if (!Helpers.TryMatchPath(path, this.options.TargetFolder))
            {
                this.logger.LogPathMismatch(path);
            }
            else
            {
                // get the image location on disk
                var imagePath = Path.Combine(
                    this.env.WebRootPath,
                    path.Value
                        .Replace('/', Path.DirectorySeparatorChar)
                        .TrimStart(Path.DirectorySeparatorChar)
                );


                if (!File.Exists(imagePath))
                {
                    this.logger.LogProcessingImage(path.Value);

                    var imageSourcePath = Path.Combine(
                        this.env.ContentRootPath + this.options.SourceFolder,
                        path.Value
                            .Replace(this.options.TargetFolder, "")
                            .Replace('/', Path.DirectorySeparatorChar)
                            .TrimStart(Path.DirectorySeparatorChar)
                    );

                    var targetDir = Path.Combine(
                        this.env.WebRootPath,
                        this.options.TargetFolder
                            .Replace('/', Path.DirectorySeparatorChar)
                            .TrimStart(Path.DirectorySeparatorChar)
                    );

                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }

                    using (var image = new MagickImage(imageSourcePath))
                    {
                        image.Resize(300, 0);
                        image.Write(imagePath);
                    }
                }
            }
            await this.next(context);
        }
    }
}
