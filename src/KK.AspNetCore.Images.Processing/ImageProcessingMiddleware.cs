namespace KK.AspNetCore.Images.Processing
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
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
            this.next = next;
            this.options = options;
            this.env = env;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            // hand to next middleware if we are not dealing with an image
            if (!this.IsImagePath(path))
            {
                await this.next(context);
                return;
            }

            // get the image location on disk
            var imagePath = Path.Combine(
                this.env.WebRootPath,
                path.Value.Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));


            if(!File.Exists(imagePath))
            {
                this.logger.LogInformation($"Processing Image: {path.Value}");
                var imageSourcePath = Path.Combine(
                    this.env.ContentRootPath + Path.DirectorySeparatorChar + this.options.SourceFolder,
                    path.Value.Replace("/" + this.options.TargetFolder, "").Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));

                var targetDir = Path.Combine(this.env.WebRootPath, this.options.TargetFolder);
                if(!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                // TODO: Do some ImageMagick
                File.Copy(imageSourcePath,imagePath);
            }
            await this.next(context);
        }

        private bool IsImagePath(PathString path)
        {
            if (path == null || !path.HasValue)
            {
                return false;
            }
            return path.Value.StartsWith($"/{this.options.TargetFolder}/");
        }

    }
}
