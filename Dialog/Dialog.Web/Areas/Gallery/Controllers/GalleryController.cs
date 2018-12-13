using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dialog.ViewModels.Gallery;
using Dialog.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dialog.Services.Contracts;

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

            return this.View(model);
        }

        public IActionResult Upload()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Upload(List<IFormFile> files)
        {
            this._galleryService.Upload(files);

            return this.View();
        }
    }
}