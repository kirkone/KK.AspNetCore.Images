namespace KK.AspNetCore.Images.Processing
{
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseImageProcessing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageProcessingMiddleware>();
        }

        public static IApplicationBuilder UseImageProcessing(
            this IApplicationBuilder builder,
            ImageProcessingOptions options
        )
        {
            return builder.UseMiddleware<ImageProcessingMiddleware>(options);
        }
    }
}
