using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.ViewModels.Base
{
    public class AllViewModel<T>
    {
        public int PageSize { get; } = 3;

        public int Page { get; set; } = 1;

        public int TotalPages { get; set; }

        public bool HasPreviousPage => this.Page > 1;

        public bool HasNextPage => this.Page < this.TotalPages;

        public string Author { get; set; }

        public ICollection<T> Entities { get; set; }
    }
}