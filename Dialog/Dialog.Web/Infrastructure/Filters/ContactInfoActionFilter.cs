using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;

namespace Dialog.Web.Infrastructure.Filters
{
    public class ContactInfoActionFilter : Attribute, IActionFilter
    {
        private readonly IConfiguration _configuration;

        public ContactInfoActionFilter(IConfiguration configuration)
        {
            this._configuration = configuration;
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
                        Address = "Sredec, 8300, Lilqna Dimitrova 1 str.",
                        Phone = "0888072710",
                        Email = "aguzelov@outlook.com",
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