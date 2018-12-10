using System.Collections.Generic;
using Dialog.Data.Models.Blog;
using Microsoft.AspNetCore.Identity;

namespace Dialog.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Posts = new HashSet<Post>();
            this.News = new HashSet<News.News>();
        }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<News.News> News { get; set; }
    }
}