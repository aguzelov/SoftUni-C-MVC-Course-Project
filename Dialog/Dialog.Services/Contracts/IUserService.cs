using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface IUserService
    {
        ICollection<T> All<T>();

        ICollection<T> AuthorWithPostsCount<T>();

        ICollection<T> AuthorWithNewsCount<T>();

        int Count();

        Task<string> GetUserRoles(string email);
    }
}