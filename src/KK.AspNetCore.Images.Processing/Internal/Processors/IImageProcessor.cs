namespace KK.AspNetCore.Images.Processing.Internal.Processors
{
    public interface IImageProcessor
    {
        ImageMagick.MagickFormat Format { get; }
    }
}
