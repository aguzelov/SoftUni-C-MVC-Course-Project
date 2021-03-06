﻿using Dialog.Data.Models.Gallery;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Dialog.Services.Contracts
{
    public interface IGalleryService
    {
        ICollection<Image> All();

        Image Upload(ICollection<IFormFile> files);

        int Count();

        Image GetDefaultImage(ImageDefaultType type);

        IQueryable<T> RecentImages<T>(int count);
    }
}