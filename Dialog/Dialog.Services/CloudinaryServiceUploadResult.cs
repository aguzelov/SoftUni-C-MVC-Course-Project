using Dialog.Services.Contracts;
using System;

namespace Dialog.Services
{
    public class CloudinaryServiceUploadResult : ICloudinaryServiceUploadResult
    {
        public string PublicId { get; set; }
        public Uri Uri { get; set; }
        public Uri SecureUri { get; set; }
    }
}