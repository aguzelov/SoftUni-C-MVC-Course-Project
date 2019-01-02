using Microsoft.AspNetCore.Http;

namespace Dialog.ViewModels.Gallery
{
    public class UploadFileViewModel
    {
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public IHeaderDictionary Headers { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}