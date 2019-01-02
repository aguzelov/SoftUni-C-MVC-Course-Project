using System.Collections.Generic;

namespace Dialog.ViewModels.Administration
{
    public class AdministrationEntityViewModel<TEntity, TAuthors>
    {
        public ICollection<TEntity> Entities { get; set; }

        public ICollection<TAuthors> Authors { get; set; }
    }
}