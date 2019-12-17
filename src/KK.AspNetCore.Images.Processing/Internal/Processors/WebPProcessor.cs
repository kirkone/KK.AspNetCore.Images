namespace KK.AspNetCore.Images.Processing.Internal.Processors
{
    using ImageMagick;

    public class WebPProcessor : IImageProcessor
    {
        public MagickFormat Format => ImageMagick.MagickFormat.WebP;
    }
}
