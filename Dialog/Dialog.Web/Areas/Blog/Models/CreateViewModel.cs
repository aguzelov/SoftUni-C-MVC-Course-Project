using System.ComponentModel.DataAnnotations;

namespace Dialog.Web.Areas.Blog.Models
{
    public class CreateViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}