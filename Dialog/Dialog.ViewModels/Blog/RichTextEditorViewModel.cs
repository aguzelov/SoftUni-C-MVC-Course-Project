using System.ComponentModel.DataAnnotations;

namespace Dialog.ViewModels.Blog
{
    public class RichTextEditorViewModel
    {
        //[AllowHtml]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}