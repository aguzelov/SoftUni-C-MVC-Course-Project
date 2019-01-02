using Dialog.Common.Mapping;
using Dialog.Data.Models.Gallery;

namespace Dialog.ViewModels.Gallery
{
    public class ImageViewModel : IMapFrom<Image>
    {
        public string Id { get; set; }

        public string SecureUri { get; set; }

        public string Name { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }
    }
}