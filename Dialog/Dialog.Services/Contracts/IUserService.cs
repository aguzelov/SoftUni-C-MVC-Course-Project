using System.Collections.Generic;
using System.Text;

namespace Dialog.Services.Contracts
{
    public interface IUserService
    {
        ICollection<T> AuthorPosts<T>();
    }
}