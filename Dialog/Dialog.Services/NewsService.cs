using AutoMapper;
using Dialog.Data;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.News;
using Microsoft.AspNetCore.Identity;

namespace Dialog.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsService(IRepository<News> newsRepository, UserManager<ApplicationUser> userManager)
        {
            this._newsRepository = newsRepository;
            this._userManager = userManager;
        }

        public AllViewModel<NewsSummaryViewModel> All(AllViewModel<NewsSummaryViewModel> model)
        {
            IQueryable<News> news = this._newsRepository.AllWithoutDeleted();

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
                 .Skip((model.Page - 1) * model.PageSize)
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
            var posts = this._newsRepository.AllWithoutDeleted()
                .OrderByDescending(p => p.CreatedOn)
                .To<T>()
                .ToList();

            return posts;
        }

        public async Task<IServiceResult> Create(string authorId, string title, string content)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            var author = await this._userManager.FindByIdAsync(authorId);

            if (author == null ||
                title == null ||
                content == null)
            {
                return result;
            }

            var news = new News
            {
                Title = title,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Author = author,
            };

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

            return model;
        }

        public ICollection<T> RecentNews<T>()
        {
            var blogs = this._newsRepository.AllWithoutDeleted()
                .OrderByDescending(p => p.CreatedOn)
                .Take(3)
                .To<T>()
                .ToList();

            return blogs;
        }

        public IQueryable<T> Search<T>(string searchTerm)
        {
            var news = this._newsRepository.AllWithoutDeleted()
                .Where(n => n.Title.Contains(searchTerm))
                .OrderByDescending(n => n.CreatedOn)
                .To<T>();

            return news;
        }

        public async Task Delete(string id)
        {
            var news = await this._newsRepository.GetByIdAsync(id);

            news.IsDeleted = true;
            news.DeletedOn = DateTime.UtcNow;

            await this._newsRepository.SaveChangesAsync();
        }

        public int Count()
        {
            var count = this._newsRepository.AllWithoutDeleted().Count();

            return count;
        }
    }
}