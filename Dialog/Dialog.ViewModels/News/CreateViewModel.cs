using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dialog.Common.Mapping;
using Microsoft.AspNetCore.Http;

namespace Dialog.ViewModels.News
{
    public class CreateViewModel : IMapFrom<Data.Models.News.News>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        [DataType(DataType.ImageUrl)]
        public ICollection<IFormFile> UploadImages { get; set; }
    }
}