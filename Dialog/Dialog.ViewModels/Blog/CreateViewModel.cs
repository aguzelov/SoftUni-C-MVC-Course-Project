using Dialog.Common.Mapping;
using Dialog.Data.Models.Blog;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dialog.ViewModels.Blog
{
    public class CreateViewModel : IMapFrom<Post>
    {
        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [DataType(DataType.ImageUrl)]
        public ICollection<IFormFile> UploadImages { get; set; }
    }
}