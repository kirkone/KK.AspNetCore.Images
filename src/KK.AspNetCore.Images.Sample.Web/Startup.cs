namespace KK.AspNetCore.Images.Sample.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using KK.AspNetCore.Images.Processing;
    using KK.AspNetCore.Images.TagHelpers;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.Extensions.FileProviders;
    using System.IO;
    using KK.AspNetCore.Images.Sample.Web.Services;

    public class Startup
    {
        public Startup(
            IConfiguration configuration
        )
        {
            this.configuration = configuration;
        }

        private IConfiguration configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services
        )
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add options for ImageProcessing to the DI
            // user either custom settings:
            // services.AddImageProcessing(options => 
            //     {
            //         options.SourceFolder = configuration["ImageProcessing:SourceFolder"] ?? "Images";
            //         options.TargetFolder = configuration["ImageProcessing:TargetFolder"] ?? "images/generated";
            //     }
            // );
            // or load settings from config provider: 
            // services.AddImageProcessingSettings(this.configuration);

            services.AddImageProcessingSettings(this.configuration);

            services.AddPictureTagSettings(this.configuration);

            services.AddSingleton<IImagesService, ImagesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }


            app.UseImageProcessing();

            // Static files should be handled after ImageProcessing so the generated files will be there already.
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
