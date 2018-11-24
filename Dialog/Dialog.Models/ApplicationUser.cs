using Dialog.Models.Blog;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Dialog.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Posts = new HashSet<Post>();
        }

        public virtual ICollection<Post> Posts { get; set; }
    }
}