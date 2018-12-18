using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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