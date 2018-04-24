namespace KK.AspNetCore.Images.Processing
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public static class ImageProcessingMiddlewareExtensions
    {
        public static IApplicationBuilder UseImageProcessing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageProcessingMiddleware>(new ImageProcessingMiddlewareOptions());
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
