namespace KK.AspNetCore.Images.Samples.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using KK.AspNetCore.Images.Samples.Web.Models;
    using KK.AspNetCore.Images.Samples.Web.Services;
    using System;

    public class HomeController : Controller
    {
        private readonly IImagesService imagesService;

        public HomeController(IImagesService imagesService) => this.imagesService = imagesService;

        public IActionResult Index() => this.View(this.imagesService.Images);

        [Route("/Details/{image}", Name = "ImageDetails")]
        public IActionResult Details(string image)
        {
            this.ViewBag.Image = image;
            return this.View();
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return this.View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";

            return this.View();
        }

        public IActionResult Privacy() => this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => this.View(
                new ErrorViewModel {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier
                }
            );
    }
}
