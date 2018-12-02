using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Dialog.Web.Infrastructure.Filters
{
    public class RecentBlogsActionFilter : Attribute, IActionFilter
    {
        private readonly IBlogService blogService;

        public RecentBlogsActionFilter(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;

            if (controller != null)
            {
                var recentBlogs = this.blogService.RecentBlogs<RecentBlogViewModel>();

                controller.ViewData["RecentBlogs"] = recentBlogs.ToList();
            }
        }
    }
}