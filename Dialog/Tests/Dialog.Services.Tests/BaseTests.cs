using System;
using System.Collections.Generic;
using System.Linq;
using Dialog.Common.Mapping;
using Dialog.ViewModels.Base;
using System.Reflection;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;
using Dialog.Data.Models.Chat;
using Dialog.Data.Models.Gallery;
using Dialog.Services.Contracts;
using NUnit.Framework;

namespace Dialog.Services.Tests
{
    public class BaseTests<T>
    {
        protected readonly string IncorrectSuffix = "Incorrect";

        protected T Service { get; set; }
        protected IQueryable<ApplicationUser> UserData { get; set; }
        protected IQueryable<Post> PostsData { get; set; }
        protected IQueryable<Comment> CommentData { get; set; }
        protected IQueryable<Chat> ChatData { get; set; }
        protected IQueryable<ChatLine> ChatLineData { get; set; }
        protected IQueryable<Image> ImageData { get; set; }

        public BaseTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);
        }

        [SetUp]
        public void Setup()
        {
            this.PostsData = new List<Post>
            {
                new Post {
                    Id = "FirstPostId",
                    Title = "First Title",
                    Content = "First Post Content",
                    Author = new ApplicationUser
                    {
                        Id = "fistUserId",
                        Email = "first@user.com",
                        UserName= "first@user.com"
                    },
                    CreatedOn = DateTime.UtcNow.AddDays(0),
                    Comments = new List<Comment>(){
                        new Comment
                        {
                            Id = "firstComment",
                            Content = "First Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "First Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "secondComment",
                            Content = "Second Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Second Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "thirdComment",
                            Content = "Third Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Third Comment Author",
                            Replies = new List<Comment>()
                        }
                    },
                },
                new Post {
                    Id = "SecondPostId",
                    Title = "Second Title",
                    Content = "Second Post Content",
                    Author = new ApplicationUser
                    {
                        Id = "secondUserId",
                        Email = "second@user.com",
                        UserName= "second@user.com"
                    },
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    Comments = new List<Comment>(){
                        new Comment
                        {
                            Id = "firstComment",
                            Content = "First Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "First Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "secondComment",
                            Content = "Second Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Second Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "thurdComment",
                            Content = "Thurd Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Thurd Comment Author",
                            Replies = new List<Comment>()
                        }
                    },
                },
                new Post {
                    Id = "ThirdPostId",
                    Title = "Third Title",
                    Content = "Third Post Content",
                    Author = new ApplicationUser
                    {
                        Id = "thirdUserId",
                        Email = "third@user.com",
                        UserName= "third@user.com"
                    },
                    CreatedOn = DateTime.UtcNow.AddDays(-2),
                    Comments = new List<Comment>(){
                        new Comment
                        {
                            Id = "firstComment",
                            Content = "First Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "First Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "secondComment",
                            Content = "Second Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Second Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "thirdComment",
                            Content = "Third Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Third Comment Author",
                            Replies = new List<Comment>()
                        }
                    },
                },
            }.AsQueryable();

            this.UserData = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "fistUserId",
                    Email = "first@user.com",
                    UserName = "first@user.com",
                    CreatedOn = DateTime.UtcNow.AddDays(0),
                    Posts = new List<Post>
                    {
                        new Post {
                    Id = "FirstPostId",
                    Title = "First Title",
                    Content = "First Post Content",
                    Author = new ApplicationUser
                    {
                        Id = "fistUserId",
                        Email = "first@user.com",
                        UserName= "first@user.com"
                    },
                    CreatedOn = DateTime.UtcNow.AddDays(0),
                    Comments = new List<Comment>(){
                        new Comment
                        {
                            Id = "firstComment",
                            Content = "First Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "First Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "secondComment",
                            Content = "Second Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Second Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "thirdComment",
                            Content = "Third Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Third Comment Author",
                            Replies = new List<Comment>()
                        }
                    },
                }
                    }
                },
                new ApplicationUser
                {
                    Id = "secondUserId",
                    Email = "second@user.com",
                    UserName= "second@user.com",
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    Posts = new List<Post>
                    {
                        new Post {
                    Id = "SecondPostId",
                    Title = "Second Title",
                    Content = "Second Post Content",
                    Author = new ApplicationUser
                    {
                        Id = "secondUserId",
                        Email = "second@user.com",
                        UserName= "second@user.com"
                    },
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    Comments = new List<Comment>(){
                        new Comment
                        {
                            Id = "firstComment",
                            Content = "First Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "First Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "secondComment",
                            Content = "Second Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Second Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "thirdComment",
                            Content = "Third Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Third Comment Author",
                            Replies = new List<Comment>()
                        }
                    },
                }
                    }
                },
                new ApplicationUser
                {
                    Id = "thirdUserId",
                    Email = "third@user.com",
                    UserName= "third@user.com",
                    CreatedOn = DateTime.UtcNow.AddDays(-2),
                    Posts = new List<Post>
                    {
                        new Post
                        {
                    Id = "ThirdPostId",
                    Title = "Third Title",
                    Content = "Third Post Content",
                    Author = new ApplicationUser
                    {
                        Id = "thirdUserId",
                        Email = "third@user.com",
                        UserName= "third@user.com"
                    },
                    CreatedOn = DateTime.UtcNow.AddDays(-2),
                    Comments = new List<Comment>(){
                        new Comment
                        {
                            Id = "firstComment",
                            Content = "First Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "First Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "secondComment",
                            Content = "Second Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Second Comment Author",
                            Replies = new List<Comment>()
                        },
                         new Comment
                        {
                            Id = "thirdComment",
                            Content = "Third Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Third Comment Author",
                            Replies = new List<Comment>()
                        }
                    },
                        }
                    }
                },
                new ApplicationUser
                {
                    Id = "fourthUserId",
                    Email = "fourth@user.com",
                    UserName= "fourth@user.com",
                    CreatedOn = DateTime.UtcNow.AddDays(-3),
                },
            }.AsQueryable();

            this.CommentData = new List<Comment>().AsQueryable();

            GenerateChatData();

            GenerateUserChats();

            GenerateImageData();
        }

        private void GenerateImageData()
        {
            var images = new List<Image>();

            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var image = new Image
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "ImageName" + i,
                    CreatedOn = DateTime.UtcNow.AddDays(random.Next(1, 10) * -1),
                };

                images.Add(image);
            }

            this.ImageData = images.AsQueryable();
        }

        private void GenerateChatData()
        {
            var chats = new List<Chat>();

            var random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var chat = new Chat
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedOn = DateTime.UtcNow.AddDays(random.Next(1, 100) * -1),
                    Name = nameof(Chat) + i,
                };

                chat.ChatLines = GenerateChatLines(chat, random.Next(3, 5));

                chats.Add(chat);
            }

            this.ChatData = chats.AsQueryable();
            this.ChatLineData = this.ChatData.SelectMany(c => c.ChatLines);
        }

        private ICollection<ChatLine> GenerateChatLines(Chat chat, int count)
        {
            var lines = new List<ChatLine>();

            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var line = new ChatLine()
                {
                    Id = Guid.NewGuid().ToString(),
                    ChatId = chat.Id,
                    Chat = chat,
                    CreatedOn = DateTime.UtcNow.AddDays(random.Next(1, 100) * -1),
                    Text = "ChatText" + i,
                    ApplicationUser = (this.UserData.ToList())[random.Next(0, this.UserData.Count())]
                };

                lines.Add(line);
            }

            return lines;
        }

        private void GenerateUserChats()
        {
            var random = new Random();

            foreach (var user in this.UserData)
            {
                var chats = this.ChatData.Where(c => c.ChatLines.Any(cl => cl.ApplicationUserId == user.Id));

                var userChats = new List<UserChat>();

                foreach (var chat in chats)
                {
                    var userChat = new UserChat()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ApplicationUser = user,
                        ApplicationUserId = user.Id,
                        Chat = chat,
                        ChatId = chat.Id,
                        CreatedOn = DateTime.UtcNow.AddDays(random.Next(1, 100) * -1)
                    };

                    userChats.Add(userChat);
                }

                user.UserChats = userChats;
            }
        }
    }
}