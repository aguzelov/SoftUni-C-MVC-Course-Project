using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface INewsService
    {
        AllViewModel<NewsSummaryViewModel> All(AllViewModel<NewsSummaryViewModel> model);

        Task<T> Details<T>(string id);

        Task<IServiceResult> Create(string authorId, string title, string content);

        ICollection<T> RecentNews<T>();

        ICollection<T> Search<T>(string searchTerm);
    }
}