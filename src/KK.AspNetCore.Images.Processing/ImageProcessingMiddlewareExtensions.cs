namespace KK.AspNetCore.Images.Processing
{
    using Microsoft.AspNetCore.Builder;

    public static class ImageProcessingMiddlewareExtensions
    {
        public static IApplicationBuilder UseImageProcessing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageProcessingMiddleware>();
        }

        public static IApplicationBuilder UseImageProcessing(
            this IApplicationBuilder builder,
            ImageProcessingMiddlewareOptions options
        )
        {
            return builder.UseMiddleware<ImageProcessingMiddleware>(options);
        }
    }
}
