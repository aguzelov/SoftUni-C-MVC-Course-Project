using System;
using System.Collections.Generic;
using Dialog.Data.Common.Models;

namespace Dialog.Data.Models.Blog
{
    public class Post : BaseDeletableModel<string>
    {
        public Post()
        {
            base.Id = Guid.NewGuid().ToString();
        }


        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}