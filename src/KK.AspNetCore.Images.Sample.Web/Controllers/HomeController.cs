namespace KK.AspNetCore.Images.Sample.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using KK.AspNetCore.Images.Sample.Web.Models;
    using KK.AspNetCore.Images.Sample.Web.Services;

    public class HomeController : Controller
    {
        private readonly IImagesService imagesService;
        public HomeController(IImagesService imagesService)
        {
            this.imagesService = imagesService;
        }
        public IActionResult Index()
        {
            ViewBag.Images = this.imagesService.Images;
            return View();
        }

        [Route("/Details/{image}", Name = "ImageDetails")]
        public IActionResult Details(string image)
        {
            ViewBag.Image = image;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
