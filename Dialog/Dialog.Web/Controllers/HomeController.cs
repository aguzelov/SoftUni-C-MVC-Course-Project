﻿using System;
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
using Microsoft.Extensions.Caching.Memory;

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
        private readonly IMemoryCache _memoryCache;

        public HomeController(IConfiguration configuration, IBlogService blogService, INewsService newsService, IGalleryService galleryService, IQuestionService questionService, ISettingsService settingsService, IMemoryCache memoryCache)
        {
            this._configuration = configuration;
            this._blogService = blogService;
            this._newsService = newsService;
            this._galleryService = galleryService;
            this._questionService = questionService;
            this._settingsService = settingsService;
            this._memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!this._memoryCache.TryGetValue(GlobalConstants.IndexRecentEntities, out IndexViewModel<PostSummaryViewModel, ImageViewModel, NewsSummaryViewModel> model))
            {
                model = new IndexViewModel<PostSummaryViewModel, ImageViewModel, NewsSummaryViewModel>
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

                var cacheOption = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(GlobalConstants.IndexRecentEntitiesCacheExpirationDay));

                this._memoryCache.Set(GlobalConstants.IndexRecentEntities, model, cacheOption);
            }

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
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            this._questionService.Add(model);

            return RedirectToAction(nameof(Contact));
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