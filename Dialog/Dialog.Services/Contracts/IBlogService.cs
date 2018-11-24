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

        IServiceResult Create(string authorId, string title, string content, string tagsString);

        IServiceResult AddComment(string postId, string authorName, string authorEmail, string message);
    }
}