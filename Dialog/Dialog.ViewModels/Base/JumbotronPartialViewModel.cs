namespace Dialog.ViewModels.Base
{
    public class JumbotronPartialViewModel
    {
        public string Title { get; set; }

        public string Parent { get; set; }
        public string ParentName { get; set; }

        public string Current { get; set; }
        public string CurrentName { get; set; }

        public bool HasSearchField { get; set; }

        public string ImageUri { get; set; }
    }
}