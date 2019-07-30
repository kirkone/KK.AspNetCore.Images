namespace KK.AspNetCore.Images.TagHelpers
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class ImageTagOptionsExtensions
    {
        /// <summary>
        /// Add the settings from "ImageTag" of the appsettings as a Singleton of ImageTagOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddImageTagSettings(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var section = configuration.GetSection("ImageTag");
            var settings = new ImageTagOptions();
            new ConfigureFromConfigurationOptions<ImageTagOptions>(section)
                .Configure(settings);

            return services.AddSingleton(settings);
        }

        /// <summary>
        /// Add the settings from "ImageTag" of the appsettings as a Singleton of ImageTagOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddImageTagSettings(
            this IServiceCollection services,
            ImageTagOptions settings
        ) => services.AddSingleton(settings);
    }
}
