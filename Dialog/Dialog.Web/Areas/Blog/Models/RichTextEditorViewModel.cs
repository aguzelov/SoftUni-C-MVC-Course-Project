using System.ComponentModel.DataAnnotations;

namespace Dialog.Web.Areas.Blog.Models
{
    public class RichTextEditorViewModel
    {
        //[AllowHtml]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}