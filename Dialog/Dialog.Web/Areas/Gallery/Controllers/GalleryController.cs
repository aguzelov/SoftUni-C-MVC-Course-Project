using Dialog.Services.Contracts;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Dialog.Web.Areas.Gallery.Controllers
{
    [Area("Gallery")]
    public class GalleryController : BaseController
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            this._galleryService = galleryService;
        }

        public IActionResult Index()
        {
            var model = this._galleryService.All();

            return View(model);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(List<IFormFile> files)
        {
            this._galleryService.Upload(files);

            return View();
        }
    }
}