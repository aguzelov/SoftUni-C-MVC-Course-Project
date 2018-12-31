using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;
using Dialog.Data.Models.Gallery;
using Dialog.Services;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dialog.Tests
{
    [TestFixture]
    public class BlogServiceTests : BaseTests
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

            this.CommentData = new List<Comment>().AsQueryable();
        }

        [Test]
        public void BlogServiceGetAllPostsWithModel()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);
            //Act
            var model = new AllViewModel<PostSummaryViewModel>();
            var posts = this.Service.All(model);

            //Assert
            Assert.AreEqual(this.PostsData.Count(), posts.Entities.Count);
            Assert.IsTrue(posts.Entities.First().CreatedOn > posts.Entities.Skip(1).First().CreatedOn);
            Assert.AreEqual(posts.TotalPages, (int)Math.Ceiling(this.PostsData.Count() / (double)model.PageSize));
            Assert.That(posts.Entities, Is.All.InstanceOf<PostSummaryViewModel>());
        }

        [Test]
        public void BlogServiceGetAllPostsWithAuthor()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);
            //Act

            var model = new AllViewModel<PostSummaryViewModel> { Author = this.PostsData.First().Author.UserName };
            var posts = this.Service.All(model);

            //Assert
            Assert.That(posts.Entities.All(p => p.AuthorName == model.Author));
            Assert.AreEqual(1, posts.Entities.Count);
            Assert.AreEqual(posts.TotalPages, (int)Math.Ceiling(this.PostsData.Count() / (double)model.PageSize));
            Assert.That(posts.Entities, Is.All.InstanceOf<PostSummaryViewModel>());
        }

        [Test]
        public void BlogServiceGetAllPosts()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);
            //Act
            var posts = this.Service.All<PostSummaryViewModel>().ToList();

            //Assert
            Assert.AreEqual(this.PostsData.Count(), posts.Count);
            Assert.IsTrue(posts[0].CreatedOn > posts[1].CreatedOn);
            Assert.IsInstanceOf<ICollection<PostSummaryViewModel>>(posts);
        }

        [Test]
        public void BlogServiceGetRecentPosts()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);

            //Act
            var posts = this.Service.RecentBlogs<RecentBlogViewModel>().ToList();
            var expectingCount = 3;

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
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });
            this.Service = new BlogService(postRepository.Object, null, null, null);

            //Act
            var post = this.Service.Details(expectedPost.Id);

            //Assert
            Assert.IsInstanceOf<PostViewModel>(post);
            Assert.AreEqual(expectedPost.Id, post.Id);
            Assert.AreEqual(expectedPost.Author.UserName, post.Author.UserName);
        }

        [Test]
        public void BlogServiceDetailsPostWithIncorrectPostId()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });
            this.Service = new BlogService(postRepository.Object, null, null, null);

            //Act
            var incorrectId = this.PostsData.First().Id + "WRONG";
            var post = this.Service.Details(incorrectId);

            //Assert
            Assert.IsNull(post);
        }

        [Test]
        public void BlogServiceCountAllPosts()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);

            //Act
            var expected = this.PostsData.Count();
            var count = this.Service.Count();

            //Assert
            Assert.AreEqual(expected, count);
        }

        [Test]
        public void BlogServiceDeletePost()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository
                .Setup(r => r.Delete(It.IsAny<Post>())).Callback((Post p) =>
                {
                    p.IsDeleted = true;
                });
            postRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });

            this.Service = new BlogService(postRepository.Object, null, null, null);

            var post = this.PostsData.First();

            //Act
            this.Service.Delete(post.Id);

            //Assert
            Assert.AreEqual(true, post.IsDeleted);
        }

        [Test]
        public void BlogServiceDeletePostWithIncorrectPostId()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository
                .Setup(r => r.Delete(It.IsAny<Post>())).Callback((Post p) =>
                {
                    p.IsDeleted = true;
                });
            postRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });

            this.Service = new BlogService(postRepository.Object, null, null, null);

            var post = this.PostsData.First();

            //Act
            this.Service.Delete(post.Id + "WRONG");

            //Assert
            Assert.AreEqual(false, post.IsDeleted);
        }

        [Test]
        public void BlogServiceSearchCurrentPost()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);
            //Act
            var expected = this.PostsData.First();

            var post = this.Service.Search(expected.Title);

            //Assert

            Assert.AreEqual(expected.Title, post.Entities.First().Title);
            Assert.IsInstanceOf<ICollection<PostSummaryViewModel>>(post.Entities);
        }

        [Test]
        public void BlogServiceSearchMultiplePosts()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);

            var searchingWord = "Title";

            //Act
            var post = this.Service.Search(searchingWord);

            //Assert
            Assert.AreEqual(this.PostsData.Count(), post.Entities.Count);
            Assert.IsTrue(post.Entities.First().CreatedOn > post.Entities.Skip(1).First().CreatedOn);
            Assert.That(post.Entities.All(p => p.Title.Contains(searchingWord)));
            Assert.That(post.Entities, Is.All.InstanceOf<PostSummaryViewModel>());
        }

        [Test]
        public void BlogServiceSearchWithIncorrectTerm()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.All()).Returns(this.PostsData);
            this.Service = new BlogService(postRepository.Object, null, null, null);

            var searchingWord = "TitleWRONG";

            //Act
            var post = this.Service.Search(searchingWord);

            //Assert
            Assert.AreEqual(0, post.Entities.Count);
        }

        [Test]
        public void BlogServiceCreatePostWithEmptyModel()
        {
            //Arrange

            this.Service = new BlogService(null, null, null, null);

            //Act
            var author = this.UserData.First();
            var result = this.Service.Create(author.Id, new CreateViewModel()).GetAwaiter().GetResult();
            var expectedErrorMsg = "Model is empty!";

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [Test]
        public void BlogServiceCreatePostWithIncorrectAuthorId()
        {
            //Arrange

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.UserData.FirstOrDefault(u => u.Id == stringed));
                });

            this.Service = new BlogService(null, null, userRepository.Object, null);

            //Act
            var author = this.UserData.First();
            var model = new CreateViewModel() { Title = "Test Title", Content = "Test Content" };

            var result = this.Service.Create(author.Id + "WRONG", model).GetAwaiter().GetResult();
            var expectedErrorMsg = "Author is not found!";

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [Test]
        public void BlogServiceCreatePost()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(p => p.Add(It.IsAny<Post>())).Callback((Post p) =>
            {
                var posts = this.PostsData.ToList();
                posts.Add(p);
                this.PostsData = posts.AsQueryable();
            });
            postRepository.Setup(p => p.SaveChangesAsync()).Returns(Task.FromResult(1));

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.UserData.FirstOrDefault(u => u.Id == stringed));
                });

            var galleryService = new Mock<IGalleryService>();
            galleryService
                .Setup(g => g.Upload(It.IsAny<ICollection<IFormFile>>()))
                .Returns(new List<Image>
            {
                    new Image(),
                    new Image()
            });

            this.Service = new BlogService(postRepository.Object, null, userRepository.Object, galleryService.Object);

            //Act
            var author = this.UserData.First();
            var model = new CreateViewModel() { Title = "Test Title", Content = "Test Content" };
            var result = this.Service.Create(author.Id, model).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.PostsData.Last().Title, model.Title);
            Assert.AreEqual(this.PostsData.Last().Content, model.Content);
        }

        [Test]
        public void BlogServiceAddComment()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });

            var commentRepository = new Mock<IDeletableEntityRepository<Comment>>();
            commentRepository.Setup(p => p.Add(It.IsAny<Comment>())).Callback((Comment c) =>
            {
                var comments = this.CommentData.ToList();
                comments.Add(c);
                this.CommentData = comments.AsQueryable();
            });
            commentRepository.Setup(p => p.SaveChangesAsync()).Returns(Task.FromResult(1));
            this.Service = new BlogService(postRepository.Object, commentRepository.Object, null, null);
            //Act

            var expectedCount = this.CommentData.Count() + 1;
            var postToAddComment = this.PostsData.First();
            string authorName = "Test Author";
            string commentContent = "Test Comment";
            var result = this.Service.AddComment(postToAddComment.Id, authorName, commentContent).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedCount, this.CommentData.Count());
            Assert.AreEqual(this.CommentData.Last().Author, authorName);
            Assert.AreEqual(this.CommentData.Last().Content, commentContent);
        }

        [Test]
        public void BlogServiceAddCommentWithIncorrectPostId()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });

            this.Service = new BlogService(postRepository.Object, null, null, null);
            //Act

            var expectedMsg = "Post not found!";
            var postToAddComment = this.PostsData.First();
            string authorName = "Test Author";
            string commentContent = "Test Comment";

            var result = this.Service.AddComment(postToAddComment.Id + "WRONG", authorName, commentContent).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMsg, result.Error);
        }

        [Test]
        public void BlogServiceAddCommentWithIncorrectParameters()
        {
            //Arrange

            this.Service = new BlogService(null, null, null, null);
            //Act

            var expectedMsg = "Invalid data!";
            var result = this.Service.AddComment(null, null, null).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMsg, result.Error);
        }

        [Test]
        public void BlogServiceEditPost()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });
            postRepository.Setup(p => p.SaveChangesAsync()).Returns(Task.FromResult(1));

            var galleryService = new Mock<IGalleryService>();
            galleryService
                .Setup(g => g.Upload(It.IsAny<ICollection<IFormFile>>()))
                .Returns(new List<Image>
                {
                    new Image(),
                    new Image()
                });

            this.Service = new BlogService(postRepository.Object, null, null, galleryService.Object);

            //Act
            var author = this.UserData.First();
            var model = new PostViewModel()
            {
                Id = this.PostsData.First().Id,
                Title = "Test Title",
                Content = "Test Content"
            };
            var result = this.Service.Edit(model).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.PostsData.First().Title, model.Title);
            Assert.AreEqual(this.PostsData.First().Content, model.Content);
            Assert.IsTrue(this.PostsData.First().ModifiedOn != null);
            Assert.IsTrue(this.PostsData.First().Image != null);
        }

        [Test]
        public void BlogServiceEditPostWithIncorrectParameters()
        {
            //Arrange

            this.Service = new BlogService(null, null, null, null);

            //Act
            var expectedMsg = "Invalid post data!";
            var model = new PostViewModel()
            {
            };
            var result = this.Service.Edit(model).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMsg, result.Error);
        }

        [Test]
        public void BlogServiceEditPostWithIncorrectPostId()
        {
            //Arrange
            var postRepository = new Mock<IDeletableEntityRepository<Post>>();
            postRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .Returns((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return Task.FromResult(this.PostsData.FirstOrDefault(p => p.Id == stringed));
                });
            this.Service = new BlogService(postRepository.Object, null, null, null);

            //Act
            var expectedMsg = "Invalid post id!";
            var model = new PostViewModel()
            {
                Id = this.PostsData.First().Id + "WRONG",
                Title = "Test Title",
                Content = "Test Content"
            };
            var result = this.Service.Edit(model).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMsg, result.Error);
        }
    }
}