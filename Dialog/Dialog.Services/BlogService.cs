using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dialog.Data;
using Dialog.Models;
using Dialog.Models.Blog;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialog.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BlogService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public AllViewModel<PostSummaryViewModel> All(AllViewModel<PostSummaryViewModel> model)
        {
            IQueryable<Post> posts = null;

            if (!string.IsNullOrEmpty(model.Author))
            {
                posts = this.context.Posts
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.Author.UserName == model.Author);
            }
            else
            {
                posts = this.context.Posts
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
            var blogs = this.context.Posts
                .OrderByDescending(p => p.CreatedOn)
                .Take(3)
                .ToList();

            var model = blogs
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return model.AsQueryable<T>();
        }

        public T Details<T>(string id)
        {
            var post = this.context.Posts
                .FirstOrDefault(p => p.Id == id);

            var models = this.mapper.Map<T>(post);

            return models;
        }

        public IServiceResult Create(string authorId, string title, string content)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            var author = this.context.Users.FirstOrDefault(u => u.Id == authorId);

            if (author == null ||
                title == null ||
                context == null)
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
                this.context.Posts.Add(post);
                this.context.SaveChanges();
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                return result;
            }

            result.Success = true;

            return result;
        }

        public IServiceResult AddComment(string postId, string authorName, string message)
        {
            var result = new ServiceResult { Success = false };

            var post = this.context.Posts.FirstOrDefault(p => p.Id == postId);
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
                this.context.Comments.Add(comment);
                this.context.SaveChanges();
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
            var post = this.context.Posts
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