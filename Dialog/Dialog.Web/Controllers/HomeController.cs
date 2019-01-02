using System.Collections.Generic;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Linq;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Blog;
using Dialog.ViewModels.Gallery;
using Dialog.ViewModels.News;

namespace Dialog.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IBlogService _blogService;
        private readonly INewsService _newsService;
        private readonly IGalleryService _galleryService;

        public HomeController(IConfiguration configuration, IBlogService blogService, INewsService newsService, IGalleryService galleryService)
        {
            this._configuration = configuration;
            this._blogService = blogService;
            this._newsService = newsService;
            this._galleryService = galleryService;
        }

        public IActionResult Index()
        {
            this.ViewData["Title"] = "Dialog";

            var model = new IndexViewModel<PostSummaryViewModel, ImageViewModel, NewsSummaryViewModel>();

            model.Posts = this._blogService.RecentBlogs<PostSummaryViewModel>().ToList();
            model.News = this._newsService.RecentNews<NewsSummaryViewModel>().ToList();
            model.Images = this._galleryService.RecentImages<ImageViewModel>().ToList();
            return View(model);
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";
            this.ViewData["AppID"] = this._configuration.GetSection("HEREMap").GetSection("AppID").Value;
            this.ViewData["AppCode"] = this._configuration.GetSection("HEREMap").GetSection("AppCode").Value;
            var model = new ContactViewModel
            {
                Address = "Sredec, 8300, Lilqna Dimitrova 1 str.",
                Phone = "0888072710",
                Email = "aguzelov@outlook.com",
                HereAppId = this._configuration.GetSection("HEREMap").GetSection("AppID").Value,
                HereAppCode = this._configuration.GetSection("HEREMap").GetSection("AppCode").Value
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}