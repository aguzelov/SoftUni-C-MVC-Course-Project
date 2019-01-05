using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Dialog.Common;

namespace Dialog.Web.Areas.News.Controllers
{
    [Area("News")]
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISettingsService _settingsService;

        public NewsController(INewsService newsService, UserManager<ApplicationUser> userManager, ISettingsService settingsService)
        {
            this._newsService = newsService;
            this._userManager = userManager;
            this._settingsService = settingsService;
        }

        public IActionResult All(AllViewModel<NewsSummaryViewModel> model)
        {
            model.PageSize = int.Parse(this._settingsService.Get(GlobalConstants.AllEntitiesPageSizeKey));

            model = this._newsService.All(model);

            return View(model);
        }

        public IActionResult Details(string id)
        {
            var model = this._newsService.Details(id);

            if (model != null)
            {
                return View(model);
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Create()
        {
            var model = new CreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            var user = this.User;
            var authorId = this._userManager.GetUserId(user);

            var result = await this._newsService.Create(authorId, model);

            if (!result.Success)
            {
                return View(model);
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(All));
            }

            var model = this._newsService.Search(searchTerm);

            if (model == null ||
                model.Entities.Count == 0)
            {
                return RedirectToAction(nameof(All));
            }

            return View("All", model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this._newsService.Delete(id);

            return RedirectToAction("News", GlobalConstants.AdministratorController, new { area = GlobalConstants.AdministrationArea });
        }

        public IActionResult Edit(string id)
        {
            var post = this._newsService.Details(id);

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(NewsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this._newsService.Edit(model);

            if (!result.Success)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { Id = model.Id });
        }
    }
}