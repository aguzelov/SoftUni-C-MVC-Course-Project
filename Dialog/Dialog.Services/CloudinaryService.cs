using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dialog.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Dialog.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this._cloudinary = cloudinary;
        }

        public ICloudinaryServiceUploadResult Upload(IFormFile file, string fileExtension)
        {
            ICloudinaryServiceUploadResult uploadResult = new CloudinaryServiceUploadResult();
            //Assigning Unique Filename (Guid)
            var myUniqueFileName = Convert.ToString(Guid.NewGuid());

            // concatenating  FileName + FileExtension
            var newFileName = myUniqueFileName + fileExtension;

            using (var fs = new MemoryStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(newFileName, file.OpenReadStream())
                };

                var result = this._cloudinary.Upload(uploadParams);
                if (result != null)
                {
                    uploadResult.PublicId = result.PublicId;
                    uploadResult.Uri = result.Uri;
                    uploadResult.SecureUri = result.SecureUri;
                }
            }

            return uploadResult;
        }
    }
}