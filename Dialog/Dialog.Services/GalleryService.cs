using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models.Gallery;
using Dialog.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Transformation = CloudinaryDotNet.Transformation;

namespace Dialog.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IRepository<Image> _imageRepository;

        public GalleryService(Cloudinary cloudinary, IRepository<Image> imageRepository)
        {
            this._cloudinary = cloudinary;
            this._imageRepository = imageRepository;
        }

        public ICollection<Image> All()
        {
            var images = this._imageRepository.All().ToList();

            return images;
        }

        public ICollection<Image> Upload(ICollection<IFormFile> files)
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

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        // concating  FileName + FileExtension
                        var newFileName = myUniqueFileName + fileExtension;

                        using (var fs = new MemoryStream())
                        {
                            var uploadParams = new ImageUploadParams()
                            {
                                File = new FileDescription(newFileName, file.OpenReadStream())
                            };

                            var uploadResult = this._cloudinary.Upload(uploadParams);

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
                                TransformationType = Data.Models.Gallery.Transformation.Fit
                            };
                            images.Add(image);
                            this._imageRepository.Add(image);
                        }
                    }
                }

                this._imageRepository.SaveChangesAsync().GetAwaiter().GetResult();
            }

            return images;
        }

        public int Count()
        {
            var count = this._imageRepository.All().Count();

            return count;
        }
    }
}