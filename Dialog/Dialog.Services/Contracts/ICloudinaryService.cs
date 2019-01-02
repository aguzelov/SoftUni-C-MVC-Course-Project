using Microsoft.AspNetCore.Http;

namespace Dialog.Services.Contracts
{
    public interface ICloudinaryService
    {
        ICloudinaryServiceUploadResult Upload(IFormFile file, string fileExtension);
    }
}