using Dialog.Common;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Administration;
using Dialog.ViewModels.Blog;
using Dialog.ViewModels.News;
using Dialog.ViewModels.User;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdminRole)]
    public class AdministratorController : BaseController
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;
        private readonly INewsService _newsService;
        private readonly IGalleryService _galleryService;

        public AdministratorController(IBlogService blogService, IUserService userService, INewsService newsService, IGalleryService galleryService)
        {
            this._blogService = blogService;
            this._userService = userService;
            this._newsService = newsService;
            this._galleryService = galleryService;
        }

        public IActionResult Index()
        {
            var model = new AdministratorIndexViewModel
            {
                PostsCount = this._blogService.Count(),
                NewsCount = this._newsService.Count(),
                UsersCount = this._userService.Count(),
                ImagesCount = this._galleryService.Count(),
            };

            return this.View(model);
        }

        public IActionResult Blog()
        {
            var model = new AdministrationEntityViewModel<PostSummaryViewModel, AuthorsWithPostsCountViewModel>
            {
                Entities = this._blogService.All<PostSummaryViewModel>(),
                Authors = this._userService.AuthorWithPostsCount<AuthorsWithPostsCountViewModel>()
            };
            return this.View(model);
        }

        public IActionResult News()
        {
            var model = new AdministrationEntityViewModel<NewsSummaryViewModel, AuthorsWithNewsCountViewModel>
            {
                Entities = this._newsService.All<NewsSummaryViewModel>(),
                Authors = this._userService.AuthorWithNewsCount<AuthorsWithNewsCountViewModel>()
            };
            return this.View(model);
        }

        public IActionResult Users()
        {
            var users = this._userService.All<UserSummaryViewModel>().ToList();

            users.ForEach(u => u.Role = this._userService.GetUserRoles(u.Email).GetAwaiter().GetResult());

            var model = new AdministrationUsersViewModel
            {
                Users = users
            };

            return this.View(model);
        }

        public async Task<IActionResult> ApproveUser(string id)
        {
            await this._userService.Approve(id);

            return this.RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> PromoteUser(string id)
        {
            await this._userService.Promote(id);

            return this.RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> DemoteUser(string id)
        {
            await this._userService.Demote(id);

            return this.RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            await this._userService.DeleteUser(id);

            return this.RedirectToAction(nameof(Users));
        }

        public IActionResult Gallery()
        {
            var model = this._galleryService.All();

            return this.View(model);
        }
    }
}