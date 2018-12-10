using Dialog.ViewModels.Base;
using System.Collections.Generic;

namespace Dialog.ViewModels.Blog
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public AuthorViewModel Author { get; set; }

        public ICollection<TagViewModel> Tags { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }

        public CreateCommentViewModel CreateCommentViewModel { get; set; }
    }
}