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
        private readonly INewsService _newsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsController(INewsService newsService, UserManager<ApplicationUser> userManager)
        {
            this._newsService = newsService;
            this._userManager = userManager;
        }

        public IActionResult All(AllViewModel<NewsSummaryViewModel> model)
        {
            model = this._newsService.All(model);

            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            var model = this._newsService.Details(id);

            if (model != null)
            {
                return this.View(model);
            }

            return this.RedirectToAction(nameof(All));
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
            var authorId = this._userManager.GetUserId(user);

            var result = await this._newsService.Create(authorId, model);

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

            var model = this._newsService.Search(searchTerm);

            if (model == null ||
                model.Entities.Count == 0)
            {
                return this.RedirectToAction(nameof(All));
            }

            return this.View("All", model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this._newsService.Delete(id);

            return RedirectToAction("News", "Administrator", new { area = "Administration" });
        }

        public IActionResult Edit(string id)
        {
            var post = this._newsService.Details(id);

            return this.View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this._newsService.Edit(model);

            if (!result.Success)
            {
                return this.View(model);
            }

            return this.RedirectToAction(nameof(Details), new { Id = model.Id });
        }
    }
}