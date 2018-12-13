﻿using System;
using System.Collections.Generic;
using System.Text;
using Dialog.Data.Models.Gallery;
using Microsoft.AspNetCore.Http;

namespace Dialog.Services.Contracts
{
    public interface IGalleryService
    {
        ICollection<Image> All();

        void Upload(List<IFormFile> files);

        int Count();
    }
}