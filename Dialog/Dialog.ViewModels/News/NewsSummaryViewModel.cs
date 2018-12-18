using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dialog.Common.Mapping;
using Dialog.ViewModels.Gallery;

namespace Dialog.ViewModels.News
{
    public class NewsSummaryViewModel : IMapFrom<Data.Models.News.News>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

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

        public ImageViewModel Image { get; set; }

        public string ModifiedDateText => this.ModifiedOn.Equals(default(DateTime))
            ? "Not Modified"
            : this.ModifiedOn.ToString("dd.MM.yyyy");
    }
}