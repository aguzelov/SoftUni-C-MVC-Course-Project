using Dialog.Models;
using Dialog.Services.Contracts;
using Dialog.Web.Areas.Blog.Models;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

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

        public IActionResult All()
        {
            var model = this.blogService.All<PostSummaryViewModel>();

            return this.View(model);
        }

        public IActionResult ByAuthor(string author)
        {
            var model = this.blogService.All<PostSummaryViewModel>(author);

            return this.View("All", model);
        }

        public IActionResult ByDate(DateTime date)
        {
            var model = this.blogService.All<PostSummaryViewModel>(date);

            return this.View("All", model);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.RedirectToAction(nameof(All));
            }

            var model = this.blogService.Details<PostViewModel>(id);

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

            var result = this.blogService.Create(authorId, model.Title, model.Content);

            if (!result.Success)
            {
                return this.View(model);
            }

            return this.RedirectToAction(nameof(All));
        }

        public IActionResult AddComment(CreateCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(Details), model.PostId);
            }

            var result = this.blogService.AddComment(model.PostId, model.Author, model.Message);

            if (!result.Success)
            {
                return this.RedirectToAction(nameof(Details), model.PostId);
            }

            return this.RedirectToAction(actionName: nameof(Details), routeValues: new { id = model.PostId });
        }
    }
}