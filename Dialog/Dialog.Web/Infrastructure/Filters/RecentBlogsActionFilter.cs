using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using Dialog.Common;

namespace Dialog.Web.Infrastructure.Filters
{
    public class RecentBlogsActionFilter : Attribute, IActionFilter, IPageFilter
    {
        private readonly IBlogService blogService;
        private readonly ISettingsService _settingsService;

        public RecentBlogsActionFilter(IBlogService blogService, ISettingsService settingsService)
        {
            this.blogService = blogService;
            this._settingsService = settingsService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                var recentBlogs = this.blogService
                    .RecentBlogs<RecentBlogViewModel>(
                        int.Parse(this._settingsService.Get(GlobalConstants.RecentPostsCountKey)));

                controller.ViewData["RecentBlogs"] = recentBlogs.ToList();
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            var result = context.Result;
            if (result is PageResult pageResult)
            {
                var viewData = pageResult.ViewData;
                var recentBlogs = this.blogService.RecentBlogs<RecentBlogViewModel>(
                    int.Parse(this._settingsService.Get(GlobalConstants.RecentPostsCountKey)));

                viewData["RecentBlogs"] = recentBlogs.ToList();
            }
        }
    }
}