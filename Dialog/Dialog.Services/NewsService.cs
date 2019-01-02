using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Gallery;
using Dialog.Data.Models.News;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Gallery;
using Dialog.ViewModels.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services
{
    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> _newsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> _userRepository;
        private readonly IGalleryService _galleryService;

        public NewsService(IDeletableEntityRepository<News> newsRepository, IDeletableEntityRepository<ApplicationUser> userRepository, IGalleryService galleryService)
        {
            this._newsRepository = newsRepository;
            this._userRepository = userRepository;
            this._galleryService = galleryService;
        }

        public AllViewModel<NewsSummaryViewModel> All(AllViewModel<NewsSummaryViewModel> model)
        {
            var news = this._newsRepository.All();

            if (!string.IsNullOrEmpty(model.Author))
            {
                news = news
                    .OrderByDescending(p => p.CreatedOn)
                    .Where(p => p.Author.UserName == model.Author);
            }
            else
            {
                news = news
                    .OrderByDescending(p => p.CreatedOn);
            }

            var currentNews = news
                 .Skip((model.CurrentPage - 1) * model.PageSize)
                 .Take(model.PageSize)
                 .To<NewsSummaryViewModel>()
                 .ToList();

            var totalNews = news.Count();

            model.TotalPages = (int)Math.Ceiling(totalNews / (double)model.PageSize);

            model.Entities = currentNews;

            return model;
        }

        public ICollection<T> All<T>()
        {
            var posts = this._newsRepository.All()
                .OrderByDescending(p => p.CreatedOn)
                .To<T>()
                .ToList();

            return posts;
        }

        public async Task<IServiceResult> Create(string authorId, CreateViewModel model)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            if (string.IsNullOrEmpty(authorId) ||
                string.IsNullOrEmpty(model.Title) ||
                string.IsNullOrEmpty(model.Content))
            {
                result.Error = "Invalid is parameters!";
                return result;
            }

            var author = await this._userRepository.GetByIdAsync(authorId);

            if (author == null)
            {
                result.Error = "Author not found!";
                return result;
            }

            var news = new News
            {
                Title = model.Title,
                Content = model.Content,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Author = author,
            };

            if (model.UploadImages != null)
            {
                var image = this._galleryService.Upload(model.UploadImages);

                if (image != null)
                {
                    news.Image = image;
                }
            }

            if (news.Image == null)
            {
                news.Image = this._galleryService.GetDefaultImage(ImageDefaultType.News);
            }

            try
            {
                this._newsRepository.Add(news);
                await this._newsRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                return result;
            }

            result.Success = true;

            return result;
        }

        public NewsViewModel Details(string id)
        {
            var news = this._newsRepository.GetByIdAsync(id).GetAwaiter().GetResult();

            if (news == null)
            {
                return null;
            }

            var model = new NewsViewModel
            {
                Id = news.Id,
                Content = news.Content,
                Title = news.Title,
                Author = new AuthorViewModel
                {
                    Id = news.Author.Id,
                    UserName = news.Author.UserName
                }
            };

            if (news.Image != null)
            {
                model.Image = new ImageViewModel
                {
                    Id = news.Image.Id,
                    SecureUri = news.Image.SecureUri,
                    Name = news.Image.Name,
                    Height = news.Image.Height,
                    Width = news.Image.Width
                };
            }

            return model;
        }

        //public ICollection<T> RecentNews<T>()
        //{
        //    var blogs = this._newsRepository.All()
        //        .OrderByDescending(p => p.CreatedOn)
        //        .Take(3)
        //        .To<T>()
        //        .ToList();

        //    return blogs;
        //}

        public AllViewModel<NewsSummaryViewModel> Search(string searchTerm)
        {
            var news = this._newsRepository.All()
                .Where(n => n.Title.Contains(searchTerm))
                .OrderByDescending(n => n.CreatedOn)
                .To<NewsSummaryViewModel>();

            var model = new AllViewModel<NewsSummaryViewModel>();

            var currentPosts = news
                .Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();

            var totalPosts = news.Count();

            model.TotalPages = (int)Math.Ceiling(totalPosts / (double)model.PageSize);

            model.Entities = currentPosts;

            return model;
        }

        public async Task Delete(string id)
        {
            var news = await this._newsRepository.GetByIdAsync(id);
            if (news == null)
            {
                return;
            }

            this._newsRepository.Delete(news);

            await this._newsRepository.SaveChangesAsync();
        }

        public int Count()
        {
            var count = this._newsRepository.All().Count();

            return count;
        }

        public async Task<IServiceResult> Edit(NewsViewModel model)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            if (string.IsNullOrEmpty(model.Id) ||
                string.IsNullOrEmpty(model.Title) ||
                string.IsNullOrEmpty(model.Content))
            {
                result.Error = "Invalid news data!";
                return result;
            }

            var news = await this._newsRepository.GetByIdAsync(model.Id);

            if (news == null)
            {
                result.Error = "Invalid news id!";
                return result;
            }

            var image = this._galleryService.Upload(model.UploadImages);

            news.Title = model.Title;
            news.Content = model.Content;
            if (image != null)
            {
                news.Image = image;
            }

            news.ModifiedOn = DateTime.UtcNow;

            try
            {
                await this._newsRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                return result;
            }

            result.Success = true;

            return result;
        }
    }
}