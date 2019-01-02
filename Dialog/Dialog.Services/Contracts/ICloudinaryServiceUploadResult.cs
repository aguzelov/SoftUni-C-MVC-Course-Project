using System;

namespace Dialog.Services.Contracts
{
    public interface ICloudinaryServiceUploadResult
    {
        string PublicId { get; set; }

        Uri Uri { get; set; }

        Uri SecureUri { get; set; }
    }
}