using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Dialog.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await this.Clients.Others.SendAsync("ReceiveMessage", user, message);
        }
    }
}