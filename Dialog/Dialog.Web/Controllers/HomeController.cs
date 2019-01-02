using Dialog.ViewModels.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Dialog.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            this.ViewData["Title"] = "Dialog";
            return View();
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";
            this.ViewData["AppID"] = this._configuration.GetSection("HEREMap").GetSection("AppID").Value;
            this.ViewData["AppCode"] = this._configuration.GetSection("HEREMap").GetSection("AppCode").Value;
            var model = new ContactViewModel
            {
                Address = "Sredec, 8300, Lilqna Dimitrova 1 str.",
                Phone = "0888072710",
                Email = "aguzelov@outlook.com",
                HereAppId = this._configuration.GetSection("HEREMap").GetSection("AppID").Value,
                HereAppCode = this._configuration.GetSection("HEREMap").GetSection("AppCode").Value
            };

            return View(model);
        }

        public IActionResult Causes()
        {
            this.ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Event()
        {
            this.ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Gallery()
        {
            this.ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}