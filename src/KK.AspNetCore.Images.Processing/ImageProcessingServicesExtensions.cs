﻿namespace KK.AspNetCore.Images.Processing
{
    using System;
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
            services.AddSingleton(settings);

            return services;
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

            services.AddSingleton(settings);

            return services;
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

            services.Configure(configureOptions);
            return services;
        }
    }
}
