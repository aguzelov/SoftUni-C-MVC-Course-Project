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
    public class ApplicationSettingsActionFilter : Attribute, IActionFilter, IPageFilter
    {
        private readonly ISettingsService _settingsService;

        public ApplicationSettingsActionFilter(ISettingsService settingsService)
        {
            this._settingsService = settingsService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                controller.ViewData[GlobalConstants.ApplicationNameKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationNameKey);
                controller.ViewData[GlobalConstants.ApplicationAboutFooterKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationAboutFooterKey);
                controller.ViewData[GlobalConstants.ApplicationAddressKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationAddressKey);
                controller.ViewData[GlobalConstants.ApplicationEmailKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationEmailKey);
                controller.ViewData[GlobalConstants.ApplicationPhoneKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationPhoneKey);
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

                viewData[GlobalConstants.ApplicationNameKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationNameKey);
                viewData[GlobalConstants.ApplicationAboutFooterKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationAboutFooterKey);
                viewData[GlobalConstants.ApplicationAddressKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationAddressKey);
                viewData[GlobalConstants.ApplicationEmailKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationEmailKey);
                viewData[GlobalConstants.ApplicationPhoneKey] =
                    this._settingsService.Get(GlobalConstants.ApplicationPhoneKey);
            }
        }
    }
}