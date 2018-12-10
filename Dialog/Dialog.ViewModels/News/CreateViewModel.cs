using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dialog.Common.Mapping;

namespace Dialog.ViewModels.News
{
    public class CreateViewModel : IMapFrom<Data.Models.News.News>
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}