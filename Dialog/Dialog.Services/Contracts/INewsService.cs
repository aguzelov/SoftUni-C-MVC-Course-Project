using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface INewsService
    {
        AllViewModel<NewsSummaryViewModel> All(AllViewModel<NewsSummaryViewModel> model);

        ICollection<T> All<T>();

        NewsViewModel Details(string id);

        Task<IServiceResult> Create(string authorId, CreateViewModel model);

        //ICollection<T> RecentNews<T>();

        AllViewModel<NewsSummaryViewModel> Search(string searchTerm);

        Task Delete(string id);

        int Count();

        Task<IServiceResult> Edit(NewsViewModel model);
    }
}