using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface INewsService
    {
        AllViewModel<NewsSummaryViewModel> All(AllViewModel<NewsSummaryViewModel> model);

        ICollection<T> All<T>();

        NewsViewModel Details(string id);

        Task<IServiceResult> Create(string authorId, CreateViewModel model);

        IQueryable<T> RecentNews<T>(int count);

        AllViewModel<NewsSummaryViewModel> Search(string searchTerm);

        Task Delete(string id);

        int Count();

        Task<IServiceResult> Edit(NewsViewModel model);
    }
}