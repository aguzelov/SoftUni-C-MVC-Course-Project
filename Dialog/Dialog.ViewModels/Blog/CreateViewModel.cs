using System.ComponentModel.DataAnnotations;

namespace Dialog.ViewModels.Blog
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