using Dialog.Common;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Administration;
using Dialog.ViewModels.Blog;
using Dialog.ViewModels.User;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dialog.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class AdministratorController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public AdministratorController(IBlogService blogService, IUserService userService)
        {
            this._blogService = blogService;
            this._userService = userService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Blog()
        {
            var model = new AdministrationBlogViewModel
            {
                Posts = this._blogService.All<PostSummaryViewModel>(),
                Authors = this._userService.AuthorPosts<AuthorsWithPostsCountViewModel>()
            };
            return this.View(model);
        }
    }
}