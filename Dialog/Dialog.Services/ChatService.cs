using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Chat;
using Dialog.Services.Contracts;

namespace Dialog.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<ChatLine> _chatLineRepository;
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<ApplicationUser> _userRepository;

        public ChatService(IRepository<ChatLine> chatLineRepository, IRepository<Chat> chatRepository, IRepository<ApplicationUser> userRepository)
        {
            this._chatLineRepository = chatLineRepository;
            this._chatRepository = chatRepository;
            this._userRepository = userRepository;
        }

        public async Task<IServiceResult> AddMessage(string chatName, string username, string message)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            var user = this._userRepository.AllWithoutDeleted().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                result.Error = "User not found!";
                return result;
            }

            var chat = this._chatRepository.AllWithoutDeleted().FirstOrDefault(c => c.Name == chatName);

            if (chat == null)
            {
                result.Error = "Chat not found!";
                return result;
            }

            var chatLine = new ChatLine
            {
                ApplicationUserId = user.Id,
                ChatId = chat.Id,
                CreatedOn = DateTime.UtcNow,
                Text = message
            };

            this._chatLineRepository.Add(chatLine);
            await this._chatLineRepository.SaveChangesAsync();

            result.Success = true;
            return result;
        }

        public IQueryable<T> RecentMessage<T>(string chatName)
        {
            var message = this._chatLineRepository.AllWithoutDeleted()
                .Where(m => m.Chat.Name == chatName && m.CreatedOn >= DateTime.UtcNow.AddDays(-1))
                .OrderBy(m => m.CreatedOn)
                .To<T>();

            return message;
        }

        public IQueryable<T> UserChats<T>(string username)
        {
            var user = this._userRepository.AllWithoutDeleted()
                .FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                throw new NullReferenceException(username);
            }

            var chats = user.UserChats.Select(uc => uc.Chat).AsQueryable().To<T>();

            return chats;
        }

        public string GetChatId(string chatName)
        {
            var chat = this._chatRepository.AllWithoutDeleted().FirstOrDefault(c => c.Name == chatName);

            if (chat == null)
            {
                throw new ArgumentException("Invalid chat name!");
            }

            return chat.Id;
        }
    }
}