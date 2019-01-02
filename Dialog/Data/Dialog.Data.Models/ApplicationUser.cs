using Dialog.Data.Common.Models;
using Dialog.Data.Models.Blog;
using Dialog.Data.Models.Chat;
using Dialog.Data.Models.Gallery;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Dialog.Data.Models
{
    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Posts = new HashSet<Post>();
            this.News = new HashSet<News.News>();
        }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<News.News> News { get; set; }
        public virtual ICollection<UserChat> UserChats { get; set; } = new HashSet<UserChat>();
    }
}