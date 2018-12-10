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
        private readonly IMapper mapper;

        public NewsService(IRepository<News> newsRepository,UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this._newsRepository = newsRepository;
            this._userManager = userManager;
            this.mapper = mapper;
        }

        public AllViewModel<NewsSummaryViewModel> All(AllViewModel<NewsSummaryViewModel> model)
        {
            IQueryable<News> news = null;

            if (!string.IsNullOrEmpty(model.Author))
            {
                news = this._newsRepository.All()
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.Author.UserName == model.Author);
            }
            else
            {
                news = this._newsRepository.All()
                .OrderByDescending(p => p.CreatedOn);
            }

            var currentNews = news
                 .Skip((model.Page - 1) * model.PageSize)
                 .Take(model.PageSize)
                 .ToList();

            var totalNews = news.Count();

            model.TotalPages = (int)Math.Ceiling(totalNews / (double)model.PageSize);

            model.Entities = currentNews.Select(p => this.mapper.Map<NewsSummaryViewModel>(p)).ToList();

            return model;
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

        public async Task<T> Details<T>(string id)
        {
            var news = await this._newsRepository.GetByIdAsync(id);

            var models = this.mapper.Map<T>(news);

            return models;
        }

        public ICollection<T> RecentNews<T>()
        {
            var blogs = this._newsRepository.All()
                .OrderByDescending(p => p.CreatedOn)
                .Take(3)
                .ToList();

            var model = blogs
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return model;
        }

        public ICollection<T> Search<T>(string searchTerm)
        {
            var news = this._newsRepository.All()
                .Where(n => n.Title.Contains(searchTerm))
                .OrderByDescending(n => n.CreatedOn)
                .ToList();

            var models = news
                .Select(n => this.mapper.Map<T>(n))
                .ToList();

            return models;
        }
    }
}