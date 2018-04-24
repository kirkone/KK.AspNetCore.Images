namespace KK.AspNetCore.Images.Processing
{
    public class ImageProcessingMiddlewareOptions
    {
        public string SourceFolder { get; set; } = "Images";
        public string TargetFolder { get; set; } = "img";
    }
}