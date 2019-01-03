using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using Dialog.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Dialog.Web.Infrastructure.Filters
{
    public class ApplicationSettingsActionFilter : Attribute, IActionFilter, IPageFilter
    {
        private readonly ISettingsService _settingsService;
        private readonly IMemoryCache _memoryCache;

        public ApplicationSettingsActionFilter(ISettingsService settingsService, IMemoryCache memoryCache)
        {
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
                if (!this._memoryCache.TryGetValue(GlobalConstants.ApplicationInfo, out ContactInfoViewModel model))
                {
                    model = new ContactInfoViewModel()
                    {
                        AppName = this._settingsService.Get(GlobalConstants.ApplicationNameKey),
                        AppFooterAboutContent = this._settingsService.Get(GlobalConstants.ApplicationAboutFooterKey),
                        AppAddress = this._settingsService.Get(GlobalConstants.ApplicationAddressKey),
                        AppEmail = this._settingsService.Get(GlobalConstants.ApplicationEmailKey),
                        AppPhone = this._settingsService.Get(GlobalConstants.ApplicationPhoneKey)
                    };

                    var cacheOption = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromDays(GlobalConstants.ApplicationInfoCacheExpirationDay));

                    this._memoryCache.Set(GlobalConstants.ApplicationInfoCacheExpirationDay, model, cacheOption);
                }

                controller.ViewData[GlobalConstants.ApplicationInfo] = model;
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

                if (!this._memoryCache.TryGetValue(GlobalConstants.ApplicationInfo, out ContactInfoViewModel model))
                {
                    model = new ContactInfoViewModel()
                    {
                        AppName = this._settingsService.Get(GlobalConstants.ApplicationNameKey),
                        AppFooterAboutContent = this._settingsService.Get(GlobalConstants.ApplicationAboutFooterKey),
                        AppAddress = this._settingsService.Get(GlobalConstants.ApplicationAddressKey),
                        AppEmail = this._settingsService.Get(GlobalConstants.ApplicationEmailKey),
                        AppPhone = this._settingsService.Get(GlobalConstants.ApplicationPhoneKey)
                    };

                    var cacheOption = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromDays(GlobalConstants.ApplicationInfoCacheExpirationDay));

                    this._memoryCache.Set(GlobalConstants.ApplicationInfoCacheExpirationDay, model, cacheOption);
                }

                viewData[GlobalConstants.ApplicationInfo] = model;
            }
        }
    }
}