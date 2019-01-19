using Dialog.Common;
using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISettingsService _settingsService;

        public BlogController(IBlogService blogService, UserManager<ApplicationUser> userManager, ISettingsService settingsService)
        {
            this._blogService = blogService;
            this._userManager = userManager;
            this._settingsService = settingsService;
        }

        public IActionResult All(AllViewModel<PostSummaryViewModel> model)
        {
            model.PageSize = int.Parse(this._settingsService.Get(GlobalConstants.AllEntitiesPageSizeKey));

            model = this._blogService.All(model);

            return View(model);
        }

        public IActionResult Details(string id)
        {
            if (id != null)
            {
                var model = this._blogService.Details(id);
                if (model != null)
                {
                    return View(model);
                }
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = new CreateViewModel();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            var user = this.User;
            var authorId = this._userManager.GetUserId(user);

            var result = await this._blogService.Create(authorId, model);

            if (!result.Success)
            {
                this.ModelState.AddModelError(GlobalConstants.ModelStateServiceResult, result.Error);
                return View(model);
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> AddComment(CreateCommentViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction(nameof(Details), routeValues: new { Id = model.PostId });
            }

            var result = await this._blogService.AddComment(model.PostId, model.Author, model.Message);

            if (!result.Success)
            {
                return RedirectToAction(nameof(Details), model.PostId);
            }

            return RedirectToAction(actionName: nameof(Details), routeValues: new { id = model.PostId });
        }

        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(All));
            }

            var model = this._blogService.Search(searchTerm);

            if (model == null ||
                !model.Entities.Any())
            {
                return RedirectToAction(nameof(All));
            }

            return View(nameof(All), model);
        }

        [Authorize(Roles = GlobalConstants.AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id != null)
            {
                await this._blogService.Delete(id);
            }

            return RedirectToAction("Blog", GlobalConstants.AdministratorController, new { area = GlobalConstants.AdministrationArea });
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var post = this._blogService.Details(id);

            return View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(PostViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this._blogService.Edit(model);

            if (!result.Success)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { Id = model.Id });
        }
    }
}