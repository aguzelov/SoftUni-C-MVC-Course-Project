using System;
using System.Collections.Generic;

namespace Dialog.Web.Areas.Blog.Models
{
    public class CommentViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Author { get; set; }

        public ICollection<CommentViewModel> Replies { get; set; }
    }
}