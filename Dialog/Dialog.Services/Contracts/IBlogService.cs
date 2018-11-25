using System;
using System.Collections.Generic;

namespace Dialog.Services.Contracts
{
    public interface IBlogService
    {
        ICollection<T> All<T>();

        ICollection<T> All<T>(string authorName);

        ICollection<T> All<T>(DateTime date);

        T Details<T>(string id);

        IServiceResult Create(string authorId, string title, string content);

        IServiceResult AddComment(string postId, string authorName, string message);

        ICollection<T> RecentBlogs<T>();
    }
}