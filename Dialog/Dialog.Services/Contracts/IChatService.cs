using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface IChatService
    {
        Task<IServiceResult> AddMessage(string chatName, string username, string message);

        IQueryable<T> RecentMessage<T>(string chatName);

        IQueryable<T> UserChats<T>(string username);

        string GetChatId(string chatName);
    }
}