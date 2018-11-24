namespace Dialog.Web.Areas.Blog.Models
{
    public class CreateCommentViewModel
    {
        public string PostId { get; set; }

        public string Author { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }
    }
}