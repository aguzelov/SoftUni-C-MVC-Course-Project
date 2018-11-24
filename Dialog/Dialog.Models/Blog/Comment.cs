using Dialog.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Dialog.Models.Blog
{
    public class Comment : IAuditInfo, IDeletableEntity
    {
        public Comment()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Replies = new HashSet<Comment>();
        }

        public string Id { get; set; }

        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public string PostId { get; set; }
        public virtual Post Post { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }
    }
}