using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using Dialog.Common;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Dialog.Web.Infrastructure.Filters
{
    public class ContactInfoActionFilter : Attribute, IActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly ISettingsService _settingsService;
        private readonly IMemoryCache _memoryCache;

        public ContactInfoActionFilter(IConfiguration configuration, ISettingsService settingsService, IMemoryCache memoryCache)
        {
            this._configuration = configuration;
            this._settingsService = settingsService;
            this._memoryCache = memoryCache;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                if (controllerActionDescriptor.MethodInfo.Name == "Contact")
                {
                    if (!this._memoryCache.TryGetValue(GlobalConstants.ContactInfo, out ContactViewModel model))
                    {
                        var cacheOption = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(GlobalConstants.ContactInfoCacheExpirationDay));

                        model = new ContactViewModel
                        {
                            Address = this._settingsService.Get(GlobalConstants.ApplicationAddressKey),
                            Phone = this._settingsService.Get(GlobalConstants.ApplicationPhoneKey),
                            Email = this._settingsService.Get(GlobalConstants.ApplicationEmailKey),
                            HereAppId = this._configuration.GetSection("HEREMap").GetSection("AppID").Value,
                            HereAppCode = this._configuration.GetSection("HEREMap").GetSection("AppCode").Value
                        };

                        this._memoryCache.Set(GlobalConstants.ContactInfoCacheExpirationDay, model, cacheOption);
                    }

                    if (context.Controller is Controller controller) controller.ViewData[GlobalConstants.ContactInfo] = model;
                }
            }
        }
    }
}