using System;

namespace Dialog.Web.Areas.Blog.Models
{
    public class RecentBlogViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

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
    }
}