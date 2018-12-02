using System.ComponentModel.DataAnnotations;

namespace Dialog.ViewModels.Blog
{
    public class CreateCommentViewModel
    {
        [Required]
        public string PostId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Author")]
        public string Author { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}