using AutoMapper;
using Dialog.Data;
using Dialog.Models.Blog;
using Dialog.Services.Contracts;
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

        public ICollection<T> All<T>()
        {
            var posts = this.context.Post.OrderByDescending(p => p.CreatedOn).ToList();

            var models = posts
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return models;
        }

        public T Details<T>(string id)
        {
            var post = this.context.Post
                .FirstOrDefault(p => p.Id == id);

            var models = this.mapper.Map<T>(post);

            return models;
        }

        public IServiceResult Create(string authorId, string title, string content, string tagsString)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            if (title == null ||
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
                AuthorId = authorId,
            };

            this.context.Post.Add(post);
            this.context.SaveChanges();

            result.Success = true;

            return result;
        }

        public IServiceResult AddComment(string postId, string authorName, string authorEmail, string message)
        {
            var result = new ServiceResult { Success = true };

            var post = this.context.Post.FirstOrDefault(p => p.Id == postId);
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

            this.context.Comments.Add(comment);
            this.context.SaveChanges();

            result.Success = true;

            return result;
        }

        public ICollection<T> All<T>(string authorName)
        {
            var posts = this.context.Post
                .Where(p => p.Author.UserName == authorName)
                .OrderByDescending(p => p.CreatedOn)
                .ToList();

            var models = posts
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return models;
        }

        public ICollection<T> All<T>(DateTime date)
        {
            var posts = this.context.Post
                .Where(p => p.CreatedOn == date)
                .OrderByDescending(p => p.CreatedOn)
                .ToList();

            var models = posts
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return models;
        }
    }
}