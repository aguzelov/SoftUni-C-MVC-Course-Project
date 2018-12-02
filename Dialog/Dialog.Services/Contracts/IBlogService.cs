using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialog.Services.Contracts
{
    public interface IBlogService
    {
        AllViewModel<PostSummaryViewModel> All(AllViewModel<PostSummaryViewModel> model);

        T Details<T>(string id);

        IServiceResult Create(string authorId, string title, string content);

        IServiceResult AddComment(string postId, string authorName, string message);

        IQueryable<T> RecentBlogs<T>();

        IQueryable<T> Search<T>(string searchTerm);
    }
}