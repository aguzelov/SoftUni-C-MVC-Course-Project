using System;

namespace Dialog.ViewModels.Blog
{
    public class PostSummaryViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CommmentsCount { get; set; }

        public string AuthorName { get; set; }

        public string ShortAuthorName
        {
            get
            {
                if (this.AuthorName.Contains("@"))
                {
                    var indexOfAt = this.AuthorName.IndexOf("@");

                    var result = this.AuthorName.Substring(0, indexOfAt);

                    return result;
                }
                else
                {
                    return this.AuthorName;
                }
            }
        }

        public string ShortContent =>
            this.Content.Length > 50 ?
            this.Content.Substring(0, 50) :
            this.Content;
    }
}