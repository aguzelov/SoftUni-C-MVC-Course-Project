using System;
using System.Collections.Generic;
using System.Text;
using Dialog.ViewModels.Blog;
using Dialog.ViewModels.User;

namespace Dialog.ViewModels.Administration
{
    public class AdministrationBlogViewModel
    {
        public ICollection<PostSummaryViewModel> Posts { get; set; }

        public ICollection<AuthorsWithPostsCountViewModel> Authors { get; set; }
    }
}