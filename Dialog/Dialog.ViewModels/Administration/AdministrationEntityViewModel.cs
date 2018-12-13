using System;
using System.Collections.Generic;
using System.Text;
using Dialog.ViewModels.Blog;
using Dialog.ViewModels.User;

namespace Dialog.ViewModels.Administration
{
    public class AdministrationEntityViewModel<TEntity, TAuthors>
    {
        public ICollection<TEntity> Entities { get; set; }

        public ICollection<TAuthors> Authors { get; set; }
    }
}