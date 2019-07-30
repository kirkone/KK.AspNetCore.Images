namespace KK.AspNetCore.Images.Samples.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using KK.AspNetCore.Images.Processing;
    using KK.AspNetCore.Images.TagHelpers;
    using KK.AspNetCore.Images.Samples.Web.Services;

    public class Startup
    {
        public Startup(
            IConfiguration configuration
        ) => this.Configuration = configuration;

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(
            IServiceCollection services
        )
        {
            _ = services.Configure<CookiePolicyOptions>(options =>
              {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                  options.MinimumSameSitePolicy = SameSiteMode.None;
              });


            _ = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Add options for ImageProcessing to the DI
            // user either custom settings:
            // _ = services.AddImageProcessing(options =>
            //     {
            //         options.SourceFolder = configuration["ImageProcessing:SourceFolder"] ?? "Images";
            //         options.TargetFolder = configuration["ImageProcessing:TargetFolder"] ?? "images/generated";
            //     }
            // );
            // or load settings from config provider:
            // _ = services.AddImageProcessingSettings(this.configuration);

            _ = services.AddImageProcessingSettings(this.Configuration);

            _ = services.AddPictureTagSettings(this.Configuration);

            _ = services.AddImageTagSettings(this.Configuration);

            _ = services.AddSingleton<IImagesService, ImagesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env
            )
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("/Home/Error")
                .UseHsts()
                .UseHttpsRedirection();
            }


            _ = app.UseImageProcessing();

            // Static files should be handled after ImageProcessing so the generated files will be there already.
            _ = app.UseStaticFiles();

            _ = app.UseCookiePolicy();

            _ = app.UseMvc(routes =>
                _ = routes.MapRoute(
                      name: "default",
                      template: "{controller=Home}/{action=Index}/{id?}")
                );
        }
    }
}
