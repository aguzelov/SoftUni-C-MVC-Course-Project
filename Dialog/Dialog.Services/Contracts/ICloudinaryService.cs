using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Dialog.Services.Contracts
{
    public interface ICloudinaryService
    {
        ICloudinaryServiceUploadResult Upload(IFormFile file, string fileExtension);
    }
}