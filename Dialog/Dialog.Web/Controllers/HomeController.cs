using System.Collections.Generic;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Linq;
using Dialog.Common;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Blog;
using Dialog.ViewModels.Gallery;
using Dialog.ViewModels.News;
using Dialog.ViewModels.Question;

namespace Dialog.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IBlogService _blogService;
        private readonly INewsService _newsService;
        private readonly IGalleryService _galleryService;
        private readonly IQuestionService _questionService;
        private readonly ISettingsService _settingsService;

        public HomeController(IConfiguration configuration, IBlogService blogService, INewsService newsService, IGalleryService galleryService, IQuestionService questionService, ISettingsService settingsService)
        {
            this._configuration = configuration;
            this._blogService = blogService;
            this._newsService = newsService;
            this._galleryService = galleryService;
            this._questionService = questionService;
            this._settingsService = settingsService;
        }

        public IActionResult Index()
        {
            var model = new IndexViewModel<PostSummaryViewModel, ImageViewModel, NewsSummaryViewModel>
            {
                Slogan = this._settingsService.Get(GlobalConstants.ApplicationSloganKey),
                Posts = this._blogService
                    .RecentBlogs<PostSummaryViewModel>(
                        int.Parse(this._settingsService.Get(GlobalConstants.IndexPostsCountKey)))
                    .ToList(),
                News = this._newsService
                    .RecentNews<NewsSummaryViewModel>(
                        int.Parse(this._settingsService.Get(GlobalConstants.IndexNewsCountKey)))
                    .ToList(),
                Images = this._galleryService
                    .RecentImages<ImageViewModel>(
                        int.Parse(this._settingsService.Get(GlobalConstants.IndexImagesCountKey)))
                    .ToList()
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var model = new QuestionViewModel();

            return View(model);
        }

        [HttpPost]
        public IActionResult Contact(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this._questionService.Add(model);

            return this.RedirectToAction(nameof(Contact));
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