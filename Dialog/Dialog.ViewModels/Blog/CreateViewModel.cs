﻿using System.ComponentModel.DataAnnotations;
using Dialog.Common.Mapping;
using Dialog.Data.Models.Blog;

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
    }
}