using Dialog.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Dialog.Models.Blog
{
    public class Post : IAuditInfo, IDeletableEntity
    {
        public Post()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Tags = new HashSet<PostsTags>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<PostsTags> Tags { get; set; }
    }
}