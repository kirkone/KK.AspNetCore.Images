namespace KK.AspNetCore.Images.Processing.Internal.Extensions
{
    using ImageMagick;
    using KK.AspNetCore.Images.Processing.Internal.Processors;

    internal static class MagickImageExtensions
    {
        public static void Process(
            this MagickImage image,
            IImageProcessor processor
        )
            => processor.Process(image);
    }
}
