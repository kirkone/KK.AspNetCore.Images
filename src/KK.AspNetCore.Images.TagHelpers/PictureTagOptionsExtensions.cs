namespace KK.AspNetCore.Images.TagHelpers
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    public static class PictureTagOptionsExtensions
    {
        /// <summary>
        /// Add the settings from "PictureTag" of the appsettings as a Singleton of PictureTagOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPictureTagSettings(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var section = configuration.GetSection("PictureTag");
            var settings = new PictureTagOptions();
            new ConfigureFromConfigurationOptions<PictureTagOptions>(section)
                .Configure(settings);
            services.AddSingleton(settings);

            return services;
        }

        /// <summary>
        /// Add the settings from "PictureTag" of the appsettings as a Singleton of PictureTagOptions
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents the root of an IConfiguration hierarchy.</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddPictureTagSettings(
            this IServiceCollection services,
            PictureTagOptions settings
        )
        {
            services.AddSingleton(settings);

            return services;
        }

    }
}
