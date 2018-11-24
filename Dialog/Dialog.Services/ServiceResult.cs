using Dialog.Services.Contracts;

namespace Dialog.Services
{
    public class ServiceResult : IServiceResult
    {
        public bool Success { get; set; }

        public string Error { get; set; }
    }
}