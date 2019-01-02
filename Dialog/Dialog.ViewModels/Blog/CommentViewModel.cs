using Dialog.Common.Mapping;
using Dialog.Data.Models.Blog;
using System;
using System.Collections.Generic;

namespace Dialog.ViewModels.Blog
{
    public class CommentViewModel : IMapFrom<Comment>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Author { get; set; }

        public ICollection<CommentViewModel> Replies { get; set; }
    }
}