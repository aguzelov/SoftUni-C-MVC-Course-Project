using Dialog.Services.Contracts;
using Dialog.ViewModels.Chat;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Dialog.Common;

namespace Dialog.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            this._chatService = chatService;
        }

        public async Task SendMessage(string chatName, string message)
        {
            string name = this.Context.User.Identity.Name;

            var result = await this._chatService.AddMessage(chatName, name, message);

            if (!result.Success)
            {
                throw new ArgumentException(result.Error);
            }

            await this.Clients.OthersInGroup(chatName).SendAsync("ReceiveMessage", chatName, name, message, DateTime.UtcNow);
            //await this.Clients.Others.SendAsync("ReceiveMessage", name, message);
        }

        public async Task SendRecentMessages(string chatName)
        {
            string name = this.Context.User.Identity.Name;

            var message = this._chatService
                .RecentMessage<RecentMessagesViewModel>(chatName)
                .ToList();

            var json = JsonConvert.SerializeObject(message);

            await this.Clients.User(name).SendAsync("GetRecentMessages", json);
        }

        public async Task SendUserChats(string username)
        {
            string name = this.Context.User.Identity.Name;

            var chats = this._chatService
                .UserChats<UserChatsViewModel>(username)
                .ToList();

            var json = JsonConvert.SerializeObject(chats);

            await this.Clients.User(name).SendAsync("GetUserChats", json);
        }

        public async Task AddToGroup(string chatName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, chatName);
        }

        public async Task RemoveFromGroup(string chatName)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, chatName);
        }

        public override async Task OnConnectedAsync()
        {
            string name = this.Context.User.Identity.Name;

            await AddToGroup(GlobalConstants.GlobalChatRoomName);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string name = this.Context.User.Identity.Name;

            await RemoveFromGroup(GlobalConstants.GlobalChatRoomName);

            await base.OnDisconnectedAsync(exception);
        }
    }
}