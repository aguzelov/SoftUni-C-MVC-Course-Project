using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Dialog.Data.Models;

namespace Dialog.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class BlogController : BaseController
    {
        private readonly IBlogService blogService;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            this.blogService = blogService;
            this.userManager = userManager;
        }

        public IActionResult All(AllViewModel<PostSummaryViewModel> model)
        {
            model = this.blogService.All(model);

            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.RedirectToAction(nameof(All));
            }

            var model =  this.blogService.Details(id);

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

            var result = await this.blogService.Create(authorId, model.Title, model.Content);

            if (!result.Success)
            {
                return this.View(model);
            }

            return this.RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> AddComment(CreateCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(Details), routeValues: new { Id = model.PostId });
            }

            var result = await this.blogService.AddComment(model.PostId, model.Author, model.Message);

            if (!result.Success)
            {
                return this.RedirectToAction(nameof(Details), model.PostId);
            }

            return this.RedirectToAction(actionName: nameof(Details), routeValues: new { id = model.PostId });
        }

        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return this.RedirectToAction(nameof(All));
            }

            var model = this.blogService.Search<PostSummaryViewModel>(searchTerm);

            if (model == null ||
                model.Count() == 0)
            {
                return this.RedirectToAction(nameof(All));
            }

            return this.View("All", model);
        }
    }
}