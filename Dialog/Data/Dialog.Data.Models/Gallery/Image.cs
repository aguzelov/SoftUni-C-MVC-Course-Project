using Dialog.Data.Common.Models;
using System;

namespace Dialog.Data.Models.Gallery
{
    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            base.Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string ContentType { get; set; }

        public string PublicId { get; set; }

        public string Uri { get; set; }

        public string SecureUri { get; set; }

        public ImageDefaultType DefaultType { get; set; }

        public Transformation TransformationType { get; set; }
    }
}