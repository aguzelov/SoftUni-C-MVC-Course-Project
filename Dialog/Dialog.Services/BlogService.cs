using AutoMapper;
using Dialog.Data;
using Dialog.Models;
using Dialog.Models.Blog;
using Dialog.Services.Contracts;
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

        public ICollection<T> All<T>()
        {
            var posts = this.context.Posts.OrderByDescending(p => p.CreatedOn).ToList();

            var models = posts
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return models;
        }

        public ICollection<T> All<T>(string authorName)
        {
            var posts = this.context.Posts
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
            var posts = this.context.Posts
                .Where(p => p.CreatedOn == date)
                .OrderByDescending(p => p.CreatedOn)
                .ToList();

            var models = posts
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return models;
        }

        public ICollection<T> RecentBlogs<T>()
        {
            var blogs = this.context.Posts
                .OrderByDescending(p => p.CreatedOn)
                .Take(3)
                .ToList();

            var model = blogs
                .Select(p => this.mapper.Map<T>(p))
                .ToList();

            return model;
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
    }
}