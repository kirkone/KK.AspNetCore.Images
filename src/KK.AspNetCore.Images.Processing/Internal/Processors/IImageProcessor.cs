namespace KK.AspNetCore.Images.Processing.Internal.Processors
{
    using ImageMagick;
    public interface IImageProcessor
    {
        ImageMagick.MagickFormat Format { get; }

        void Process(MagickImage image);
    }
}
