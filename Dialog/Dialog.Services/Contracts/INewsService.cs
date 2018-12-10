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

        NewsViewModel Details(string id);

        Task<IServiceResult> Create(string authorId, string title, string content);

        ICollection<T> RecentNews<T>();

        IQueryable<T> Search<T>(string searchTerm);
    }
}