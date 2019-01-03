using Dialog.Data.Common.Repositories;
using Dialog.Data.Models.Gallery;
using Dialog.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dialog.Common.Mapping;

namespace Dialog.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IDeletableEntityRepository<Image> _imageRepository;

        public GalleryService(ICloudinaryService cloudinaryService, IDeletableEntityRepository<Image> imageRepository)
        {
            this._cloudinaryService = cloudinaryService;
            this._imageRepository = imageRepository;
        }

        public ICollection<Image> All()
        {
            var images = this._imageRepository.All().ToList();

            return images;
        }

        public Image Upload(ICollection<IFormFile> files)
        {
            var images = new List<Image>();

            if (files.Any())
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        //Getting FileName
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim().ToString();

                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        if (!this._imageRepository.All().Any(i => i.Name == fileName))
                        {
                            var uploadResult = this._cloudinaryService.Upload(file, fileExtension);

                            var uri = uploadResult.SecureUri;

                            var image = new Image
                            {
                                PublicId = uploadResult.PublicId,
                                Uri = uploadResult.Uri.ToString(),
                                SecureUri = uploadResult.SecureUri.ToString(),
                                ContentType = fileExtension,
                                Name = fileName,
                                Width = 100,
                                Height = 150,
                                TransformationType = Data.Models.Gallery.Transformation.Fit,
                                CreatedOn = DateTime.UtcNow,
                            };
                            images.Add(image);
                            this._imageRepository.Add(image);
                        }
                        else
                        {
                            var image = this._imageRepository.All().FirstOrDefault(i => i.Name == fileName);
                            images.Add(image);
                        }
                    }
                }

                this._imageRepository.SaveChangesAsync().GetAwaiter().GetResult();
            }

            return images.FirstOrDefault();
        }

        public IQueryable<T> RecentImages<T>(int count)
        {
            var news = this._imageRepository.All()
                .OrderByDescending(p => p.CreatedOn)
                .Take(count)
                .To<T>();

            return news;
        }

        public int Count()
        {
            var count = this._imageRepository.All().Count();

            return count;
        }

        public Image GetDefaultImage(ImageDefaultType type)
        {
            var image = this._imageRepository.All().FirstOrDefault(i => i.DefaultType == type);

            return image;
        }
    }
}