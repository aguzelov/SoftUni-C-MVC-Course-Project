using Dialog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dialog.Web.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Dialog";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Blog()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult BlogSingle()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Causes()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Event()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Gallery()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}