using Dialog.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Dialog.Models.Blog
{
    public class Tag : IAuditInfo, IDeletableEntity
    {
        public Tag()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Posts = new HashSet<PostsTags>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<PostsTags> Posts { get; set; }
    }
}