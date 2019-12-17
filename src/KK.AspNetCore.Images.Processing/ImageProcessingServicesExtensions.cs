namespace KK.AspNetCore.Images.Processing
{
    using System;
    using KK.AspNetCore.Images.Processing.Internal.Processors;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class ImageProcessingServicesExtensions
    {
        /// <summary>
        /// Add the settings from "ImageProcessing" of the appsettings as a Singleton of ImageProcessingMiddlewareOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddImageProcessingSettings(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection("ImageProcessing");
            var settings = new ImageProcessingOptions();
            new ConfigureFromConfigurationOptions<ImageProcessingOptions>(section)
                .Configure(settings);

            return services.AddSingleton<IImageProcessor, JpegProcessor>()
                .AddSingleton<IImageProcessor, WebPProcessor>().AddSingleton(settings);
        }

        /// <summary>
        /// Add the settings from "ImageProcessing" of the appsettings as a Singleton of ImageProcessingMiddlewareOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddImageProcessingSettings(
            this IServiceCollection services,
            ImageProcessingOptions settings
        )
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return services
                .AddSingleton<IImageProcessor, JpegProcessor>()
                .AddSingleton<IImageProcessor, WebPProcessor>()
                .AddSingleton(settings);
        }

        /// <summary>
        /// Adds Image Processing services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureOptions">A delegate to configure the <see cref="ImageProcessingOptions"/>.</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddImageProcessing(this IServiceCollection services, Action<ImageProcessingOptions> configureOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            return services
                .AddSingleton<IImageProcessor, JpegProcessor>()
                .AddSingleton<IImageProcessor, WebPProcessor>()
                .Configure(configureOptions);
        }
    }
}
