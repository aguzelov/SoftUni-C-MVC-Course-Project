﻿using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Dialog.Common;
using Dialog.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dialog.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class BlogController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogController(IBlogService blogService, UserManager<ApplicationUser> userManager)
        {
            this._blogService = blogService;
            this._userManager = userManager;
        }

        public IActionResult All(AllViewModel<PostSummaryViewModel> model)
        {
            model = this._blogService.All(model);

            return this.View(model);
        }

        public IActionResult Details(string id)
        {
            if (id != null)
            {
                var model = this._blogService.Details(id);
                if (model != null)
                {
                    return this.View(model);
                }
            }

            return this.RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Create()
        {
            var model = new CreateViewModel();

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }
            var user = this.User;
            var authorId = this._userManager.GetUserId(user);

            var result = await this._blogService.Create(authorId, model);

            if (!result.Success)
            {
                this.ModelState.AddModelError("result", result.Error);
                return this.View(model);
            }

            return this.RedirectToAction(nameof(All));
        }

        //TODO : Add Edit Post Action

        public async Task<IActionResult> AddComment(CreateCommentViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(Details), routeValues: new { Id = model.PostId });
            }

            var result = await this._blogService.AddComment(model.PostId, model.Author, model.Message);

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

            var model = this._blogService.Search(searchTerm);

            if (model == null ||
                !model.Entities.Any())
            {
                return this.RedirectToAction(nameof(All));
            }

            return this.View("All", model);
        }

        [Authorize(Roles = GlobalConstants.AdminRole)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id != null)
            {
                await this._blogService.Delete(id);
            }

            return RedirectToAction("Blog", "Administrator", new { area = "Administration" });
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var post = this._blogService.Details(id);

            return this.View(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(PostViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this._blogService.Edit(model);

            if (!result.Success)
            {
                return this.View(model);
            }

            return this.RedirectToAction(nameof(Details), new { Id = model.Id });
        }
    }
}