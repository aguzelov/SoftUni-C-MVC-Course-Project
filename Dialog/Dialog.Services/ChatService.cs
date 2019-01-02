using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Chat;
using Dialog.Services.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<ChatLine> _chatLineRepository;
        private readonly IDeletableEntityRepository<Chat> _chatRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> _userRepository;

        public ChatService(IRepository<ChatLine> chatLineRepository, IDeletableEntityRepository<Chat> chatRepository, IDeletableEntityRepository<ApplicationUser> userRepository)
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

            var user = this._userRepository.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                result.Error = "User not found!";
                return result;
            }

            var chat = this._chatRepository.All().FirstOrDefault(c => c.Name == chatName);

            if (chat == null)
            {
                result.Error = "Chat not found!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                result.Error = "Message cannot be empty!";
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
            var message = this._chatLineRepository.All()
                .Where(m => m.Chat.Name == chatName)
                .OrderBy(m => m.CreatedOn)
                .Take(4)
                .To<T>();

            return message;
        }

        public IQueryable<T> UserChats<T>(string username)
        {
            var chats = this._userRepository.All()
                .FirstOrDefault(u => u.UserName == username)?
                .UserChats
                .Select(uc => uc.Chat)
                .AsQueryable()
                .To<T>();

            if (chats == null)
            {
                throw new ArgumentException("Invalid username");
            }

            return chats;
        }

        public string GetChatId(string chatName)
        {
            var chat = this._chatRepository.All().FirstOrDefault(c => c.Name == chatName);

            if (chat == null)
            {
                throw new ArgumentException("Invalid chat name!");
            }

            return chat.Id;
        }
    }
}