using System;
using System.Collections.Generic;
using System.Text;

namespace Dialog.ViewModels.Base
{
    public class IndexViewModel<TPosts, TImages, TNews>
    {
        public ICollection<TPosts> Posts { get; set; }
        public ICollection<TImages> Images { get; set; }
        public ICollection<TNews> News { get; set; }
    }
}