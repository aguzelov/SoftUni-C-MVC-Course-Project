using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dialog.Data;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;

namespace Dialog.Services
{
    public class BlogService : IBlogService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogService( IMapper mapper, IRepository<Post> postRepository,IRepository<Comment> commentRepository, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this._postRepository = postRepository;
            this._commentRepository = commentRepository;
            this._userManager = userManager;
        }

        public AllViewModel<PostSummaryViewModel> All(AllViewModel<PostSummaryViewModel> model)
        {
            IQueryable<Post> posts = null;

            if (!string.IsNullOrEmpty(model.Author))
            {
                posts = this._postRepository.All()
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.Author.UserName == model.Author);
            }
            else
            {
                posts = this._postRepository.All()
                .OrderByDescending(p => p.CreatedOn);
            }

            var currentPosts = posts
                 .Skip((model.Page - 1) * model.PageSize)
                 .Take(model.PageSize)
                 .ToList();

            var totalPosts = posts.Count();

            model.TotalPages = (int)Math.Ceiling(totalPosts / (double)model.PageSize);

            model.Entities = currentPosts.Select(p => this.mapper.Map<PostSummaryViewModel>(p)).ToList();

            return model;
        }

        public IQueryable<T> RecentBlogs<T>()
        {
            var blogs = this._postRepository.All()
                .OrderByDescending(p => p.CreatedOn)
                .Take(3)
                .ToList();

            var model = blogs
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return model.AsQueryable<T>();
        }

        public async Task<T> Details<T>(string id)
        {
            var post = await this._postRepository.GetByIdAsync(id);

            var models = this.mapper.Map<T>(post);

            return models;
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

            var post = new Post
            {
                Title = title,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false,
                Author = author,
            };

            try
            {
                this._postRepository.Add(post);
               await this._postRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                return result;
            }

            result.Success = true;

            return result;
        }

        public async Task<IServiceResult> AddComment(string postId, string authorName, string message)
        {
            var result = new ServiceResult { Success = false };

            var post = await this._postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                return result;
            }

            var comment = new Comment
            {
                Author = authorName,
                CreatedOn = DateTime.UtcNow,
                Post = post,
                Content = message,
                IsDeleted = false,
            };

            try
            {
                this._commentRepository.Add(comment);
               await this._commentRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                return result;
            }

            result.Success = true;

            return result;
        }

        public IQueryable<T> Search<T>(string searchTerm)
        {
            var post = this._postRepository.All()
                .Where(n => n.Title.Contains(searchTerm))
                .OrderByDescending(p => p.CreatedOn)
                .ToList();

            var models = post
                .Select(n => this.mapper.Map<T>(n))
                .ToList();

            return models.AsQueryable<T>();
        }
    }
}