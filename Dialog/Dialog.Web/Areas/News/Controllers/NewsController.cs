using System.Linq;
using System.Threading.Tasks;
using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult All(AllViewModel<NewsSummaryViewModel> model)
        {
            model = this.newsService.All(model);

            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.RedirectToAction(nameof(All));
            }

            var model = this.newsService.Details(id);

            return this.View(model);
        }

        public IActionResult Create()
        {
            var model = new CreateViewModel();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }
            var user = this.User;
            var authorId = this.userManager.GetUserId(user);

            var result = await this.newsService.Create(authorId, model.Title, model.Content);

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

            var model = this.newsService.Search<NewsSummaryViewModel>(searchTerm).ToList();

            if (model == null ||
                model.Count == 0)
            {
                return this.RedirectToAction(nameof(All));
            }

            return this.View("All", model);
        }
    }
}