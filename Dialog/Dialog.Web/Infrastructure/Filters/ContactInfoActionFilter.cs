using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using Dialog.Common;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;

namespace Dialog.Web.Infrastructure.Filters
{
    public class ContactInfoActionFilter : Attribute, IActionFilter
    {
        private readonly IConfiguration _configuration;
        private readonly ISettingsService _settingsService;

        public ContactInfoActionFilter(IConfiguration configuration, ISettingsService settingsService)
        {
            this._configuration = configuration;
            this._settingsService = settingsService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                if (controllerActionDescriptor.MethodInfo.Name == "Contact")
                {
                    var model = new ContactViewModel
                    {
                        Address = this._settingsService.Get(GlobalConstants.ApplicationAddressKey),
                        Phone = this._settingsService.Get(GlobalConstants.ApplicationPhoneKey),
                        Email = this._settingsService.Get(GlobalConstants.ApplicationEmailKey),
                        HereAppId = this._configuration.GetSection("HEREMap").GetSection("AppID").Value,
                        HereAppCode = this._configuration.GetSection("HEREMap").GetSection("AppCode").Value
                    };

                    var controller = context.Controller as Controller;

                    controller.ViewData["ContactInfo"] = model;
                }
            }
        }
    }
}