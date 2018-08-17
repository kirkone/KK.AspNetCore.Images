﻿namespace KK.AspNetCore.Images.Processing
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageMagick;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Helpers;
    using Extensions;

    public class ImageProcessingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ImageProcessingOptions options;
        private readonly ILogger<ImageProcessingMiddleware> logger;
        private readonly IHostingEnvironment env;

        public ImageProcessingMiddleware(
            RequestDelegate next,
            ImageProcessingOptions options,
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

            if (!GenericHelpers.IsGetOrHeadMethod(context.Request.Method))
            {
                this.logger.LogRequestMethodNotSupported(context.Request.Method);
            }
            else if (!GenericHelpers.TryMatchPath(path, this.options.TargetFolder))
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

                    var extension = Path.GetExtension(path.Value);
                    var size = Path.GetFileNameWithoutExtension(path.Value).ToLower();
                    var filename = Directory.GetParent(path.Value).Name;

                    var imageSourcePath = Path.Combine(
                        $"{this.env.ContentRootPath}{this.options.SourceFolder}",
                        $"{filename}{extension}"
                    );

                    var targetDir = Path.Combine(
                        this.env.WebRootPath,
                        this.options.TargetFolder
                            .Replace('/', Path.DirectorySeparatorChar)
                            .TrimStart(Path.DirectorySeparatorChar),
                        filename
                    );

                    if (!Directory.Exists(targetDir))
                    {
                        this.logger.LogInformation($"Create Directory: \"{targetDir}\"");
                        Directory.CreateDirectory(targetDir);
                    }

                    var sizeSetting = this.options.Sizes.FirstOrDefault(
                        x => (x.Name ?? x.Width.ToString()).ToLower() == size
                    );

                    if (sizeSetting == null)
                    {
                        // Not supported size!
                        // Hand over to the next middleware and return.
                        this.logger.LogSizeNotSupported(size);
                        await this.next(context);
                        return;
                    }

                    using (var image = new MagickImage(imageSourcePath))
                    {
                        image.Resize(sizeSetting.Width, sizeSetting.Height);
                        image.Strip();
                        if (sizeSetting.Quality >= 0)
                        {
                            this.logger.LogInformation($"Setting Quality to: \"{sizeSetting.Quality}\"");
                            image.Quality = sizeSetting.Quality;
                        }

                        if (sizeSetting.Progressive)
                        {
                            image.Format = MagickFormat.Pjpeg;
                        }

                        using (var stream = new MemoryStream())
                        {
                            image.Write(stream);
                            stream.Position = 0;

                            this.logger.LogInformation($"LosslessCompress before: {stream.Length / 1024} kb");
                            var imageOptimizer = new ImageOptimizer();
                            if (options.LosslessCompress)
                            {
                                imageOptimizer.LosslessCompress(stream);
                            }
                            else
                            {
                                imageOptimizer.Compress(stream);
                            }
                            this.logger.LogInformation($"LosslessCompress after: {stream.Length / 1024} kb");

                            using (
                                FileStream file = new FileStream(
                                    imagePath,
                                    FileMode.Create,
                                    System.IO.FileAccess.Write
                                )
                            )
                            {
                                stream.WriteTo(file);
                                file.Flush();
                            }
                        }
                    }
                }
            }
            await this.next(context);
        }
    }
}
