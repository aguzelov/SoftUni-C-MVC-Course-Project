using Dialog.ViewModels.Base;

namespace Dialog.ViewModels.News
{
    public class NewsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public AuthorViewModel Author { get; set; }
    }
}