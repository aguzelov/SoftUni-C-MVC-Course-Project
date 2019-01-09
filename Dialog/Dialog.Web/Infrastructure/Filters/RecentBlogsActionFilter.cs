using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using Dialog.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Dialog.Web.Infrastructure.Filters
{
    public class RecentBlogsActionFilter : Attribute, IActionFilter, IPageFilter
    {
        private readonly IBlogService blogService;
        private readonly ISettingsService _settingsService;
        private readonly IMemoryCache _memoryCache;

        public RecentBlogsActionFilter(IBlogService blogService, ISettingsService settingsService, IMemoryCache memoryCache)
        {
            this.blogService = blogService;
            this._settingsService = settingsService;
            this._memoryCache = memoryCache;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                var recentBlogs = new List<RecentBlogViewModel>();

                if (!this._memoryCache.TryGetValue(GlobalConstants.RecentBlogPost, out recentBlogs))
                {
                    recentBlogs = this.blogService
                        .RecentBlogs<RecentBlogViewModel>(
                            int.Parse(this._settingsService.Get(GlobalConstants.RecentPostsCountKey))).ToList();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(GlobalConstants.RecentBlogCacheExpirationDay));

                    this._memoryCache.Set(GlobalConstants.RecentBlogPost, recentBlogs, cacheOptions);
                }

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

                var recentBlogs = new List<RecentBlogViewModel>();

                if (!this._memoryCache.TryGetValue(GlobalConstants.RecentBlogPost, out recentBlogs))
                {
                    recentBlogs = this.blogService.RecentBlogs<RecentBlogViewModel>(
                        int.Parse(this._settingsService.Get(GlobalConstants.RecentPostsCountKey))).ToList();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromHours(GlobalConstants.RecentBlogCacheExpirationDay));

                    this._memoryCache.Set(GlobalConstants.RecentBlogPost, recentBlogs, cacheOptions);
                }

                viewData["RecentBlogs"] = recentBlogs.ToList();
            }
        }
    }
}