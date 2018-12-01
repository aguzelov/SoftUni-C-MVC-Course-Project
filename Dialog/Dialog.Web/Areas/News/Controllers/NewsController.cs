using Dialog.Models;
using Dialog.Services.Contracts;
using Dialog.Web.Areas.News.Models;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Web.Areas.News.Controllers
{
    [Area("News")]
    public class NewsController : BaseController
    {
        private readonly INewsService newsService;
        private readonly UserManager<ApplicationUser> userManager;

        public NewsController(INewsService newsService, UserManager<ApplicationUser> userManager)
        {
            this.newsService = newsService;
            this.userManager = userManager;
        }

        public IActionResult All()
        {
            var model = this.newsService.All<NewsSummaryViewModel>();

            return this.View(model);
        }

        public IActionResult ByAuthor(string author)
        {
            var model = this.newsService.All<NewsSummaryViewModel>(author);

            return this.View("All", model);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.RedirectToAction(nameof(All));
            }

            var model = this.newsService.Details<NewsViewModel>(id);

            return this.View(model);
        }

        public IActionResult Create()
        {
            var model = new CreateViewModel();

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            var user = this.User;
            var authorId = this.userManager.GetUserId(user);

            var result = this.newsService.Create(authorId, model.Title, model.Content);

            if (!result.Success)
            {
                return this.View(model);
            }

            return this.RedirectToAction(nameof(All));
        }

        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return this.RedirectToAction(nameof(All));
            }

            var model = this.newsService.Search<NewsSummaryViewModel>(searchTerm);

            if (model == null ||
                model.Count == 0)
            {
                return this.RedirectToAction(nameof(All));
            }

            return this.View("All", model);
        }
    }
}