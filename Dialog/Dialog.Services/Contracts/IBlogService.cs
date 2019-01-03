using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface IBlogService
    {
        AllViewModel<PostSummaryViewModel> All(AllViewModel<PostSummaryViewModel> model);

        ICollection<T> All<T>();

        PostViewModel Details(string id);

        Task<IServiceResult> Create(string authorId, CreateViewModel model);

        Task<IServiceResult> AddComment(string postId, string authorName, string message);

        IQueryable<T> RecentBlogs<T>(int count);

        AllViewModel<PostSummaryViewModel> Search(string searchTerm);

        Task Delete(string id);

        int Count();

        Task<IServiceResult> Edit(PostViewModel model);
    }
}