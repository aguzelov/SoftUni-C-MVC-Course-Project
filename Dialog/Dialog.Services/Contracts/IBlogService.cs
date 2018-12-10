using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface IBlogService
    {
        AllViewModel<PostSummaryViewModel> All(AllViewModel<PostSummaryViewModel> model);

        Task<T> Details<T>(string id);

        Task<IServiceResult> Create(string authorId, string title, string content);

        Task<IServiceResult> AddComment(string postId, string authorName, string message);

        IQueryable<T> RecentBlogs<T>();

        IQueryable<T> Search<T>(string searchTerm);
    }
}