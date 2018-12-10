using System;
using Dialog.Data.Common.Models;

namespace Dialog.Data.Models.News
{
    public class News : BaseDeletableModel<string>
    {
        public News()
        {
            base.Id = Guid.NewGuid().ToString();
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

    }
}