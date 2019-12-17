namespace KK.AspNetCore.Images.Processing
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageMagick;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using KK.AspNetCore.Images.Processing.Internal.Helpers;
    using KK.AspNetCore.Images.Processing.Internal.Extensions;
    using System.Collections.Generic;
    using KK.AspNetCore.Images.Processing.Internal.Processors;

    public class ImageProcessingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ImageProcessingOptions options;
        private readonly IEnumerable<IImageProcessor> imageProcessors;
        private readonly ILogger<ImageProcessingMiddleware> logger;
        private readonly IHostingEnvironment env;

        public ImageProcessingMiddleware(
            RequestDelegate next,
            ImageProcessingOptions options,
            IEnumerable<IImageProcessor> imageProcessors,
            IHostingEnvironment env,
            ILogger<ImageProcessingMiddleware> logger
        )
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.imageProcessors = imageProcessors ?? throw new ArgumentNullException(nameof(imageProcessors));
            this.env = env ?? throw new ArgumentNullException(nameof(env));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Processes a request to determine if it matches a known image, otherwise it will generate it.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            var extension = Path.GetExtension(path.Value);

            if (!GenericHelpers.TryMatchPath(path, this.options.TargetFolder))
            {
                this.logger.LogPathMismatch(path);
            }
            else if (!GenericHelpers.IsGetOrHeadMethod(context.Request.Method))
            {
                this.logger.LogRequestMethodNotSupported(context.Request.Method);
            }
            else if (!GenericHelpers.IsRequestFileTypeSupported(
                extension,
                this.options.OutputFormats.SelectMany(
                        x => x.FileEndings
                    ).ToArray()
                )
            )
            {
                this.logger.LogRequestFileTypeNotSupported(extension);
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

                    var size = Path.GetFileNameWithoutExtension(path.Value).ToLower();
                    var filename = Directory.GetParent(path.Value).Name;
                    var fileextension = Path.GetExtension(path.Value).ToLower().Trim('.');

                    FileInfo sourceImageFile;
                    var sourceImageFolder = new DirectoryInfo(Path.Combine(
                        $"{this.env.ContentRootPath}{this.options.SourceFolder}"
                    ));
                    var sourceImageFiles = sourceImageFolder.GetFiles($"{filename}.*");

                    if (sourceImageFiles == null || sourceImageFiles.Length == 0)
                    {
                        // Image source not found!
                        // Hand over to the next middleware and return.
                        this.logger.LogSourceImageNotFound($"{filename}.*");
                        await this.next(context);
                        return;
                    }
                    else
                    {
                        if (sourceImageFiles.Length > 1)
                        {
                            this.logger.LogWarning(
                                $"Found multiple source images, take first one: {sourceImageFiles[0].Name}"
                            );
                        }
                        sourceImageFile = sourceImageFiles[0];
                    }

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
                        _ = Directory.CreateDirectory(targetDir);
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

                    var sourceImageFileExtension = sourceImageFile.Extension.Trim('.');
                    var isSourceFileSupported = Enum.TryParse<MagickFormat>(sourceImageFileExtension, true, out var format);
                    if (isSourceFileSupported)
                    {
                        using (var image = new MagickImage())
                        {
                            try
                            {
                                image.Read(sourceImageFile);
                            }
                            // Something went wrong while loading the image with ImageMagick
                            catch (MagickException exception)
                            {
                                this.logger.LogError($"Error while loading image: {exception}");
                                await this.next(context);
                                return;
                            }

                            image.Resize(sizeSetting.Width, sizeSetting.Height);
                            image.Strip();
                            if (sizeSetting.Quality >= 0)
                            {
                                this.logger.LogInformation($"Setting Quality to: \"{sizeSetting.Quality}\"");
                                image.Quality = sizeSetting.Quality;
                            }

                            _ = Enum.TryParse<MagickFormat>(fileextension, true, out var outputFormat);
                            image.Format = outputFormat;

                            var imageProcessor = this.imageProcessors.Where(o => o.Format == outputFormat).FirstOrDefault();
                            if (imageProcessor == null)
                            {
                                this.logger.LogError($"No ImageProcessor found for: {outputFormat}");
                                await this.next(context);
                                return;
                            }

                            image.Process(imageProcessor);


                            if (sizeSetting.Progressive)
                            {
                                image.Format = MagickFormat.Pjpeg;
                            }
                            this.logger.LogInformation($"Image Format: {image.Format}");

                            using (var stream = new MemoryStream())
                            {
                                image.Write(stream);
                                stream.Position = 0;

                                this.logger.LogInformation($"LosslessCompress before: {stream.Length / 1024} kb");
                                var imageOptimizer = new ImageOptimizer();
                                if (this.options.LosslessCompress)
                                {
                                    _ = imageOptimizer.LosslessCompress(stream);
                                }
                                else
                                {
                                    _ = imageOptimizer.Compress(stream);
                                }
                                this.logger.LogInformation($"LosslessCompress after: {stream.Length / 1024} kb");

                                using (
                                    var file = new FileStream(
                                        imagePath,
                                        FileMode.Create,
                                        FileAccess.Write
                                    )
                                )
                                {
                                    stream.WriteTo(file);
                                    file.Flush();
                                }
                            }

                            if (this.options.WriteMetaFiles)
                            {
                                // Retrieve the exif information
                                var profile = image.GetExifProfile();

                                // Check if image contains an exif profile
                                if (profile == null)
                                {
                                    this.logger.LogInformation($"Source image has no EXIF data.");
                                }
                                else
                                {
                                    // Write all values to the console
                                    foreach (var value in profile.Values)
                                    {
                                        this.logger.LogInformation($"{value.Tag}({value.DataType}): {value.ToString()}");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this.logger.LogSourceFileTypeNotSupported($"{sourceImageFileExtension}");
                        await this.next(context);
                        return;
                    }
                }
            }
            await this.next(context);
        }
    }
}
