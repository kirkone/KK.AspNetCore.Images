namespace KK.AspNetCore.Images.Processing
{
    using System;
    using Microsoft.AspNetCore.Builder;

    public static class ImageProcessingBuilderExtensions
    {
        public static IApplicationBuilder UseImageProcessing(this IApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<ImageProcessingMiddleware>();
        }
    }
}
