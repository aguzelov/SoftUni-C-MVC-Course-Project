using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.ViewModels.News
{
    public class NewsSummaryViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ShortAuthorName
        {
            get
            {
                if (this.Author.Contains("@"))
                {
                    var indexOfAt = this.Author.IndexOf("@");

                    var result = this.Author.Substring(0, indexOfAt);

                    return result;
                }
                else
                {
                    return this.Author;
                }
            }
        }

        public string ShortContent =>
            this.Content.Length > 50 ?
            this.Content.Substring(0, 50) :
            this.Content;
    }
}