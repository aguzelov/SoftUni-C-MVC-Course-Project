using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Gallery;
using Dialog.Data.Models.News;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services.Tests
{
    [TestFixture]
    public class NewsServiceTests : BaseTests<INewsService>
    {
        [Test]
        public void NewsServiceGetAllNewsWithModel()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);

            this.Service = new NewsService(newsRepository.Object, null, null);
            //Act
            var model = new AllViewModel<NewsSummaryViewModel>();
            var news = this.Service.All(model);

            //Assert
            Assert.AreEqual(model.PageSize, news.Entities.Count);
            Assert.IsTrue(news.Entities.First().CreatedOn > news.Entities.Skip(1).First().CreatedOn);
            Assert.AreEqual(news.TotalPages, (int)Math.Ceiling(this.NewsData.Count() / (double)model.PageSize));
            Assert.That(news.Entities, Is.All.InstanceOf<NewsSummaryViewModel>());
        }

        [Test]
        public void NewsServiceGetAllNewsWithAuthor()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);
            this.Service = new NewsService(newsRepository.Object, null, null);
            //Act

            var authorName = this.NewsData.First().Author.UserName;
            var expectedNewsCount = this.NewsData.Count(n => n.Author.UserName == authorName);
            var model = new AllViewModel<NewsSummaryViewModel> { Author = authorName };
            var news = this.Service.All(model);

            //Assert
            Assert.That(news.Entities.All(p => p.Author == model.Author));
            Assert.That(news.Entities.Count <= model.PageSize);
            Assert.AreEqual(
                news.TotalPages,
                (int)Math.Ceiling(expectedNewsCount / (double)model.PageSize),
                message: $"TotalPages: {news.TotalPages} - Entities: {news.Entities.Count} - User {authorName}");
            Assert.That(news.Entities, Is.All.InstanceOf<NewsSummaryViewModel>());
        }

        [Test]
        public void NewsServiceGetAllNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);
            this.Service = new NewsService(newsRepository.Object, null, null);
            //Act
            var news = this.Service.All<NewsSummaryViewModel>().ToList();

            //Assert
            Assert.AreEqual(this.NewsData.Count(), news.Count);
            Assert.IsTrue(news[0].CreatedOn > news[1].CreatedOn);
            Assert.IsInstanceOf<ICollection<NewsSummaryViewModel>>(news);
        }

        [Test]
        public void NewsServiceGetRecentNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);
            this.Service = new NewsService(newsRepository.Object, null, null);

            //Act
            var news = this.Service.RecentNews<NewsSummaryViewModel>().ToList();
            var expectingCount = 4;

            //Assert
            Assert.AreEqual(expectingCount, news.Count);
            Assert.AreEqual(this.NewsData.OrderByDescending(n => n.CreatedOn).First().Id, news[0].Id);
            Assert.IsInstanceOf<ICollection<NewsSummaryViewModel>>(news);
        }

        [Test]
        public void NewsServiceDetailsNews()
        {
            //Arrange
            var expectedNews = this.NewsData.First();
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.NewsData.FirstOrDefault(p => p.Id == stringed);
                });
            this.Service = new NewsService(newsRepository.Object, null, null);

            //Act
            var news = this.Service.Details(expectedNews.Id);

            //Assert
            Assert.IsInstanceOf<NewsViewModel>(news);
            Assert.AreEqual(expectedNews.Id, news.Id);
            Assert.AreEqual(expectedNews.Author.UserName, news.Author.UserName);
        }

        [Test]
        public void NewsServiceDetailsNewsWithIncorrectNewsId()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.NewsData.FirstOrDefault(p => p.Id == stringed);
                });
            this.Service = new NewsService(newsRepository.Object, null, null);

            //Act

            var news = this.Service.Details(this.IncorrectTestText);

            //Assert
            Assert.IsNull(news);
        }

        [Test]
        public void NewsServiceCountAllNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);
            this.Service = new NewsService(newsRepository.Object, null, null);

            //Act
            var expected = this.NewsData.Count();
            var count = this.Service.Count();

            //Assert
            Assert.AreEqual(expected, count);
        }

        [Test]
        public void NewsServiceDeleteNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository
                .Setup(r => r.Delete(It.IsAny<News>())).Callback((News p) =>
                {
                    p.IsDeleted = true;
                });
            newsRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.NewsData.FirstOrDefault(p => p.Id == stringed);
                });

            this.Service = new NewsService(newsRepository.Object, null, null);

            var news = this.NewsData.First();

            //Act
            this.Service.Delete(news.Id);

            //Assert
            Assert.AreEqual(true, news.IsDeleted);
        }

        [Test]
        public void NewsServiceDeleteNewsWithIncorrectNewsId()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository
                .Setup(r => r.Delete(It.IsAny<News>())).Callback((News p) =>
                {
                    p.IsDeleted = true;
                });
            newsRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.NewsData.FirstOrDefault(p => p.Id == stringed);
                });

            this.Service = new NewsService(newsRepository.Object, null, null);

            var oldNews = this.NewsData;

            //Act
            this.Service.Delete(this.IncorrectTestText);

            //Assert
            CollectionAssert.AreEqual(oldNews, this.NewsData);
        }

        [Test]
        public void NewsServiceSearchCurrentNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);
            this.Service = new NewsService(newsRepository.Object, null, null);
            //Act
            var expected = this.NewsData.First();

            var news = this.Service.Search(expected.Title);

            //Assert

            Assert.AreEqual(expected.Title, news.Entities.First().Title);
            Assert.IsInstanceOf<ICollection<NewsSummaryViewModel>>(news.Entities);
        }

        [Test]
        public void NewsServiceSearchMultipleNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);
            this.Service = new NewsService(newsRepository.Object, null, null);

            var searchingWord = "Title";

            //Act
            var news = this.Service.Search(searchingWord);

            //Assert
            Assert.That(news.Entities.Count <= news.PageSize);
            Assert.IsTrue(news.Entities.First().CreatedOn > news.Entities.Skip(1).First().CreatedOn);
            Assert.That(news.Entities.All(p => p.Title.Contains(searchingWord)));
            Assert.That(news.Entities, Is.All.InstanceOf<NewsSummaryViewModel>());
        }

        [Test]
        public void NewsServiceSearchWithIncorrectTerm()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.All()).Returns(this.NewsData);
            this.Service = new NewsService(newsRepository.Object, null, null);

            //Act
            var news = this.Service.Search(this.IncorrectTestText);

            //Assert
            Assert.AreEqual(0, news.Entities.Count);
        }

        [Test]
        public void NewsServiceCreateNewsWithEmptyModel()
        {
            //Arrange

            this.Service = new NewsService(null, null, null);

            //Act
            var author = this.UserData.First();
            var result = this.Service.Create(author.Id, new CreateViewModel()).GetAwaiter().GetResult();
            var expectedErrorMsg = "Invalid is parameters!";

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [Test]
        public void NewsServiceCreateNewsWithIncorrectAuthorId()
        {
            //Arrange

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.UserData.FirstOrDefault(u => u.Id == stringed);
                });

            this.Service = new NewsService(null, userRepository.Object, null);

            //Act
            var author = this.UserData.First();
            var model = new CreateViewModel() { Title = "Test Title", Content = "Test Content" };

            var result = this.Service.Create(this.IncorrectTestText, model).GetAwaiter().GetResult();
            var expectedErrorMsg = "Author not found!";

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [Test]
        public void NewsServiceCreateNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(p => p.Add(It.IsAny<News>())).Callback((News p) =>
            {
                var news = this.NewsData.ToList();
                news.Add(p);
                this.NewsData = news.AsQueryable();
            });
            newsRepository.Setup(p => p.SaveChangesAsync()).Returns(Task.FromResult(1));

            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.UserData.FirstOrDefault(u => u.Id == stringed);
                });

            var galleryService = new Mock<IGalleryService>();
            galleryService
                .Setup(g => g.Upload(It.IsAny<ICollection<IFormFile>>()))
                .Returns(new Image());

            this.Service = new NewsService(newsRepository.Object, userRepository.Object, galleryService.Object);

            //Act
            var author = this.UserData.First();
            var model = new CreateViewModel() { Title = "Test Title", Content = "Test Content" };
            var result = this.Service.Create(author.Id, model).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.NewsData.Last().Title, model.Title);
            Assert.AreEqual(this.NewsData.Last().Content, model.Content);
        }

        [Test]
        public void NewsServiceEditNews()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.NewsData.FirstOrDefault(p => p.Id == stringed);
                });
            newsRepository.Setup(p => p.SaveChangesAsync()).Returns(Task.FromResult(1));

            var galleryService = new Mock<IGalleryService>();
            galleryService
                .Setup(g => g.Upload(It.IsAny<ICollection<IFormFile>>()))
                .Returns(new Image());

            this.Service = new NewsService(newsRepository.Object, null, galleryService.Object);

            //Act
            var model = new NewsViewModel()
            {
                Id = this.NewsData.First().Id,
                Title = "Test Title",
                Content = "Test Content"
            };
            var result = this.Service.Edit(model).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(this.NewsData.First().Title, model.Title);
            Assert.AreEqual(this.NewsData.First().Content, model.Content);
            Assert.IsTrue(this.NewsData.First().ModifiedOn != null);
            Assert.IsTrue(this.NewsData.First().Image != null);
        }

        [Test]
        public void NewsServiceEditNewsWithIncorrectParameters()
        {
            //Arrange

            this.Service = new NewsService(null, null, null);

            //Act
            var expectedMsg = "Invalid news data!";
            var model = new NewsViewModel();
            {
            };
            var result = this.Service.Edit(model).GetAwaiter().GetResult();

            //Assert
            Assert.IsInstanceOf<IServiceResult>(result);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMsg, result.Error);
        }

        [Test]
        public void NewsServiceEditNewsWithIncorrectNewsId()
        {
            //Arrange
            var newsRepository = new Mock<IDeletableEntityRepository<News>>();
            newsRepository.Setup(r => r.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var stringed = id.First().ToString();
                    return this.NewsData.FirstOrDefault(p => p.Id == stringed);
                });
            this.Service = new NewsService(newsRepository.Object, null, null);

            //Act
            var expectedMsg = "Invalid news id!";
            var model = new NewsViewModel()
            {
                Id = this.IncorrectTestText,
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