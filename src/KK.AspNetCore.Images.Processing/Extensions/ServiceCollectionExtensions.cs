namespace KK.AspNetCore.Images.Processing
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the settings from "ImageProcessing" of the appsettings as a Singleton of ImageProcessingMiddlewareOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddImageProcessingSettings(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var section = configuration.GetSection("ImageProcessing");
            var settings = new ImageProcessingOptions();
            new ConfigureFromConfigurationOptions<ImageProcessingOptions>(section)
                .Configure(settings);
            services.AddSingleton(settings);

            return services;
        }

        /// <summary>
        /// Add the settings from "ImageProcessing" of the appsettings as a Singleton of ImageProcessingMiddlewareOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddImageProcessingSettings(
            this IServiceCollection services,
            ImageProcessingOptions settings
        )
        {
            services.AddSingleton(settings);

            return services;
        }

    }
}
