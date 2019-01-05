using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Chat;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Chat;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Dialog.Common;

namespace Dialog.Services.Tests
{
    [TestFixture]
    public class ChatServiceTests : BaseTests<IChatService>
    {
        [Test]
        public void ChatServiceGetChatId()
        {
            //Arrange
            var chatRepository = new Mock<IDeletableEntityRepository<Chat>>();
            chatRepository
                .Setup(c => c.All())
                .Returns(this.ChatData);

            this.Service = new ChatService(null, chatRepository.Object, null, null);

            var expectedChat = this.ChatData.First();

            //Act
            var resultChatId = this.Service.GetChatId(expectedChat.Name);

            //Assert
            Assert.AreEqual(expectedChat.Id, resultChatId);
        }

        [Test]
        public void ChatServiceGetChatIdWithIncorrectChatName()
        {
            //Arrange
            var chatRepository = new Mock<IDeletableEntityRepository<Chat>>();
            chatRepository
                .Setup(c => c.All())
                .Returns(this.ChatData);

            this.Service = new ChatService(null, chatRepository.Object, null, null);

            //Act
            var expectedErrorMsg = string.Format(GlobalConstants.InvalidParameter, "chatName");

            //Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                this.Service.GetChatId(this.IncorrectTestText));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMsg));
        }

        [Test]
        public void ChatServiceRecentMessage()
        {
            //Arrange
            var chatLineRepository = new Mock<IRepository<ChatLine>>();
            chatLineRepository
                .Setup(c => c.All())
                .Returns(this.ChatLineData);

            this.Service = new ChatService(chatLineRepository.Object, null, null, null);

            //Act
            var searchedChat = this.ChatData.First();
            var expectedCount = this.ChatLineData.Count(c => c.Chat.Name == searchedChat.Name);

            var messages = this.Service.RecentMessage<RecentMessagesViewModel>(searchedChat.Name).ToList();

            //Assert
            Assert.IsTrue(expectedCount == messages.Count || messages.Count <= 4);
        }

        [Test]
        public void ChatServiceRecentMessageWithIncorrectChatName()
        {
            //Arrange
            var chatLineRepository = new Mock<IRepository<ChatLine>>();
            chatLineRepository
                .Setup(c => c.All())
                .Returns(this.ChatLineData);

            this.Service = new ChatService(chatLineRepository.Object, null, null, null);

            //Act

            var messages = this.Service.RecentMessage<RecentMessagesViewModel>(this.IncorrectTestText).ToList();

            //Assert
            Assert.AreEqual(0, messages.Count);
        }

        [Test]
        public void ChatServiceUserChats()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(c => c.All())
                .Returns(this.UserData);

            this.Service = new ChatService(null, null, userRepository.Object, null);

            //Act
            var user = this.UserData.First();

            var chats = this.Service.UserChats<UserChatsViewModel>(user.UserName);

            //Assert
            Assert.AreEqual(chats.Count(), user.UserChats.Count);
            Assert.That(chats, Is.All.InstanceOf<UserChatsViewModel>());
        }

        [Test]
        public void ChatServiceUserChatsWithIncorrectUsername()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(c => c.All())
                .Returns(this.UserData);

            this.Service = new ChatService(null, null, userRepository.Object, null);

            //Act
            var expectedErrorMsg = string.Format(GlobalConstants.InvalidParameter, "username");
            //Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                this.Service.UserChats<UserChatsViewModel>(this.IncorrectTestText));
            Assert.That(ex.Message, Is.EqualTo(expectedErrorMsg));
        }

        [Test]
        public void ChatServiceAddMessage()
        {
            //Arrange
            var chatRepository = new Mock<IDeletableEntityRepository<Chat>>();
            chatRepository
                .Setup(c => c.All())
                .Returns(this.ChatData);

            var chatLineRepository = new Mock<IRepository<ChatLine>>();
            chatLineRepository
                .Setup(c => c.Add(It.IsAny<ChatLine>()))
                .Callback((ChatLine line) =>
                {
                    var lines = this.ChatLineData.ToList();
                    lines.Add(line);
                    this.ChatLineData = lines.AsQueryable();
                });
            chatLineRepository.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(c => c.All())
                .Returns(this.UserData);

            this.Service = new ChatService(chatLineRepository.Object, chatRepository.Object, userRepository.Object, null);

            //Act
            var chatToAdd = this.ChatData.First();
            var userToAdd = this.UserData.First();
            var messageToAdd = "Test Message";

            var result = this.Service.AddMessage(chatToAdd.Name, userToAdd.UserName, messageToAdd).GetAwaiter()
                .GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ChatServiceAddMessageWithIncorrectUserName()
        {
            //Arrange

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(c => c.All())
                .Returns(this.UserData);

            this.Service = new ChatService(null, null, userRepository.Object, null);

            //Act
            var chatToAdd = this.ChatData.First();
            var messageToAdd = "Test Message";

            var result = this.Service.AddMessage(chatToAdd.Name, this.IncorrectTestText, messageToAdd).GetAwaiter()
                .GetResult();
            var expectedErrorMsg = string.Format(GlobalConstants.EntityIsNotFound, "User"); ;

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [Test]
        public void ChatServiceAddMessageWithIncorrectChatName()
        {
            //Arrange
            var chatRepository = new Mock<IDeletableEntityRepository<Chat>>();
            chatRepository
                .Setup(c => c.All())
                .Returns(this.ChatData);

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(c => c.All())
                .Returns(this.UserData);

            this.Service = new ChatService(null, chatRepository.Object, userRepository.Object, null);

            //Act
            var userToAdd = this.UserData.First();
            var messageToAdd = "Test Message";

            var result = this.Service.AddMessage(this.IncorrectTestText, userToAdd.UserName, messageToAdd).GetAwaiter()
                .GetResult();
            var expectedErrorMsg = string.Format(GlobalConstants.EntityIsNotFound, "Chat"); ;

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [Test]
        public void ChatServiceAddMessageWithIncorrectMessage()
        {
            //Arrange
            var chatRepository = new Mock<IDeletableEntityRepository<Chat>>();
            chatRepository
                .Setup(c => c.All())
                .Returns(this.ChatData);

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(c => c.All())
                .Returns(this.UserData);

            this.Service = new ChatService(null, chatRepository.Object, userRepository.Object, null);

            //Act
            var chatToAdd = this.ChatData.First();
            var userToAdd = this.UserData.First();

            var result = this.Service.AddMessage(chatToAdd.Name, userToAdd.UserName, null).GetAwaiter()
                .GetResult();
            var expectedErrorMsg = GlobalConstants.ModelIsEmpty;

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }
    }
}