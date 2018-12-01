using System;
using System.Collections.Generic;
using System.Text;

namespace Dialog.Services.Contracts
{
    public interface INewsService
    {
        ICollection<T> All<T>();

        ICollection<T> All<T>(string authorName);

        T Details<T>(string id);

        IServiceResult Create(string authorId, string title, string content);

        ICollection<T> RecentNews<T>();

        ICollection<T> Search<T>(string searchTerm);
    }
}