using AutoMapper;
using Dialog.Data;
using Dialog.Services;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;

namespace Tests
{
    public class BlogServiceTests
    {
        public IBlogService Service { get; set; }
        public IQueryable<Post> PostsData { get; set; }
        public IQueryable<ApplicationUser> UserData { get; set; }
        public IQueryable<Comment> CommentData { get; set; }

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
            var mockPostSet = new Mock<DbSet<Post>>();
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(this.PostsData.Provider);
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(this.PostsData.Expression);
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(this.PostsData.ElementType);
            mockPostSet.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(this.PostsData.GetEnumerator());

            this.UserData = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "fistUserId",
                    Email = "first@user.com",
                    UserName = "first@user.com",
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
                            Id = "thurdComment",
                            Content = "Thurd Comment",
                            CreatedOn = DateTime.UtcNow,
                            Author = "Thurd Comment Author",
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
                },
            }.AsQueryable();
            var mockApplicationUserSet = new Mock<DbSet<ApplicationUser>>();
            mockApplicationUserSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(this.UserData.Provider);
            mockApplicationUserSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(this.UserData.Expression);
            mockApplicationUserSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(this.UserData.ElementType);
            mockApplicationUserSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(this.UserData.GetEnumerator());

            this.CommentData = new List<Comment>().AsQueryable();
            var mockCommentSet = new Mock<DbSet<Comment>>();
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(this.CommentData.Provider);
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(this.CommentData.Expression);
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(this.CommentData.ElementType);
            mockCommentSet.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(this.CommentData.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(m => m.Posts).Returns(mockPostSet.Object);
            mockContext.Setup(m => m.Users).Returns(mockApplicationUserSet.Object);
            mockContext.Setup(m => m.Comments).Returns(mockCommentSet.Object);

            var mappingConfig = new MapperConfiguration(mc =>
                mc.AddProfile(new MappingProfile())
            );

            IMapper mapper = mappingConfig.CreateMapper();

            //this.Service = new BlogService(mockContext.Object, mapper);
        }

        [Test]
        public void AutoMapperConfigIsValid()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                // add all profiles you'll use in your project here
            });
            Mapper.AssertConfigurationIsValid();
        }

        //[Test]
        //public void BlogServiceGetAllPosts()
        //{
        //    //Arrange

        //    //Act
        //    var posts = this.Service.All<PostSummaryViewModel>().ToList();

        //    //Assert
        //    Assert.AreEqual(this.PostsData.Count(), posts.Count);
        //    Assert.IsTrue(posts[0].CreatedOn > posts[1].CreatedOn);
        //    Assert.IsInstanceOf<ICollection<PostSummaryViewModel>>(posts);
        //}

        //[Test]
        //public void BlogServiceGetAllByAuthorPosts()
        //{
        //    //Arrange
        //    var expectingPost = this.PostsData.First();
        //    var expectingCount = 1;
        //    //Act
        //    var posts = this.Service.All<PostSummaryViewModel>(expectingPost.Author.UserName).ToList();

        //    //Assert
        //    Assert.AreEqual(expectingCount, posts.Count);
        //    Assert.AreEqual(expectingPost.Id, posts[0].Id);
        //    Assert.IsInstanceOf<ICollection<PostSummaryViewModel>>(posts);
        //}

        [Test]
        public void BlogServiceGetRecentPosts()
        {
            //Arrange
            var expectingCount = 3;
            //Act
            var posts = this.Service.RecentBlogs<RecentBlogViewModel>().ToList();

            //Assert
            Assert.AreEqual(expectingCount, posts.Count);
            Assert.AreEqual(this.PostsData.First().Id, posts[0].Id);
            Assert.IsInstanceOf<ICollection<RecentBlogViewModel>>(posts);
        }

        [Test]
        public void BlogServiceDetailsPost()
        {
            //Arrange
            var expectedPost = this.PostsData.First();

            //Act
            var post = this.Service.Details(expectedPost.Id);

            //Assert
            Assert.IsInstanceOf<PostViewModel>(post);
            Assert.AreEqual(expectedPost.Id, post.Id);
            Assert.AreEqual(expectedPost.Author.UserName, post.Author.UserName);
        }

        [Test]
        public void BlogServiceCreatePost()
        {
            //Arrange
            var authorId = "fourthUserId";
            var postTitle = "Fourth Title";
            var postContent = "Fourth Post Content";

            //Act
            var result = this.Service.Create(authorId, new CreateViewModel()).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void BlogServiceAddComment()
        {
            //Arrange
            var authorName = this.UserData.First().UserName;
            var postId = this.PostsData.First().Id;
            var commentContent = "Fourth Post Content";

            //Act
            var result = this.Service.AddComment(postId, authorName, commentContent).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
        }
    }
}