using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface INewsService
    {
        AllViewModel<NewsSummaryViewModel> All(AllViewModel<NewsSummaryViewModel> model);

        ICollection<T> All<T>();

        NewsViewModel Details(string id);

        Task<IServiceResult> Create(string authorId, CreateViewModel model);

        ICollection<T> RecentNews<T>();

        IQueryable<T> Search<T>(string searchTerm);

        Task Delete(string id);

        int Count();

        Task<IServiceResult> Edit(NewsViewModel model);
    }
}