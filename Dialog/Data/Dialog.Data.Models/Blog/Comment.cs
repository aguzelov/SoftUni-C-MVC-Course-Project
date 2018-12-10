using System;
using System.Collections.Generic;
using Dialog.Data.Common.Models;

namespace Dialog.Data.Models.Blog
{
    public class Comment : BaseDeletableModel<string>
    {
        public Comment()
        {
            base.Id = Guid.NewGuid().ToString();
            this.Replies = new HashSet<Comment>();
        }
        
        public string Author { get; set; }

        public string Content { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }
    }
}