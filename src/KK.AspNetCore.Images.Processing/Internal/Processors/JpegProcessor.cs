namespace KK.AspNetCore.Images.Processing.Internal.Processors
{
    using ImageMagick;

    public class JpegProcessor: IImageProcessor
    {
        public MagickFormat Format => ImageMagick.MagickFormat.Jpg;

        public void Process(MagickImage image) => throw new System.NotImplementedException();
    }
}
