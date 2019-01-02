using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;
using Dialog.Data.Models.Gallery;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.ViewModels.Blog;
using Dialog.ViewModels.Gallery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialog.Services
{
    public class BlogService : IBlogService
    {
        private readonly IDeletableEntityRepository<Post> _postRepository;
        private readonly IDeletableEntityRepository<Comment> _commentRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> _userRepository;
        private readonly IGalleryService _galleryService;

        public BlogService(
            IDeletableEntityRepository<Post> postRepository,
            IDeletableEntityRepository<Comment> commentRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IGalleryService galleryService)
        {
            this._postRepository = postRepository;
            this._commentRepository = commentRepository;
            this._userRepository = userRepository;
            this._galleryService = galleryService;
        }

        public AllViewModel<PostSummaryViewModel> All(AllViewModel<PostSummaryViewModel> model)
        {
            var posts = this._postRepository.All();

            if (!string.IsNullOrEmpty(model.Author))
            {
                posts = posts
                .OrderByDescending(p => p.CreatedOn)
                .Where(p => p.Author.UserName == model.Author);
            }
            else
            {
                posts = posts
                .OrderByDescending(p => p.CreatedOn);
            }

            var currentPosts = posts
                 .Skip((model.CurrentPage - 1) * model.PageSize)
                 .Take(model.PageSize)
                 .To<PostSummaryViewModel>()
                 .ToList();

            var totalPosts = posts.Count();

            model.TotalPages = (int)Math.Ceiling(totalPosts / (double)model.PageSize);

            model.Entities = currentPosts;

            return model;
        }

        public ICollection<T> All<T>()
        {
            var posts = this._postRepository.All()
                 .OrderByDescending(p => p.CreatedOn)
                 .To<T>()
                 .ToList();

            return posts;
        }

        public IQueryable<T> RecentBlogs<T>()
        {
            var blogs = this._postRepository.All()
                .OrderByDescending(p => p.CreatedOn)
                .Take(4)
                .To<T>();

            return blogs;
        }

        public PostViewModel Details(string id)
        {
            var post = this._postRepository.GetByIdAsync(id).GetAwaiter().GetResult();

            if (post == null)
            {
                return null;
            }

            var model = new PostViewModel
            {
                Id = post.Id,
                Content = post.Content,
                Title = post.Title,
                Comments = post.Comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    Author = c.Author,
                    CreatedOn = c.CreatedOn,
                    Replies = new List<CommentViewModel>()
                }).ToList(),
                Author = new AuthorViewModel
                {
                    Id = post.Author.Id,
                    UserName = post.Author.UserName
                }
            };

            if (post.Image != null)
            {
                model.Image = new ImageViewModel
                {
                    Id = post.Image.Id,
                    SecureUri = post.Image.SecureUri,
                    Name = post.Image.Name,
                    Height = post.Image.Height,
                    Width = post.Image.Width
                };
            }

            return model;
        }

        public async Task<IServiceResult> Create(string authorId, CreateViewModel model)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            if (model.Title == null ||
                model.Content == null)
            {
                result.Error = "Model is empty!";
                return result;
            }

            var author = await this._userRepository.GetByIdAsync(authorId);

            if (author == null)
            {
                result.Error = "Author is not found!";
                return result;
            }

            var post = new Post
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
                    post.Image = image;
                }
            }

            if (post.Image == null)
            {
                post.Image = this._galleryService.GetDefaultImage(ImageDefaultType.BlogPost);
            }

            try
            {
                this._postRepository.Add(post);
                var affectedRows = await this._postRepository.SaveChangesAsync();
                if (affectedRows != 1)
                {
                    throw new InvalidOperationException("Post not saved in database!");
                }
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

            if (postId == null ||
                authorName == null ||
                message == null)
            {
                result.Error = "Invalid data!";
                return result;
            }

            var post = await this._postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                result.Error = "Post not found!";
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
                var affectedRows = await this._commentRepository.SaveChangesAsync();
                if (affectedRows != 1)
                {
                    throw new InvalidOperationException("Comment not saved in database!");
                }
            }
            catch (Exception e)
            {
                result.Error = e.Message;
                return result;
            }

            result.Success = true;

            return result;
        }

        public AllViewModel<PostSummaryViewModel> Search(string searchTerm)
        {
            var posts = this._postRepository.All()
                .Where(n => n.Title.Contains(searchTerm))
                .OrderByDescending(p => p.CreatedOn)
                .To<PostSummaryViewModel>();

            var model = new AllViewModel<PostSummaryViewModel>();

            var currentPosts = posts
                .Skip((model.CurrentPage - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();

            var totalPosts = posts.Count();

            model.TotalPages = (int)Math.Ceiling(totalPosts / (double)model.PageSize);

            model.Entities = currentPosts;

            return model;
        }

        public async Task Delete(string id)
        {
            var post = await this._postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return;
            }

            this._postRepository.Delete(post);

            await this._postRepository.SaveChangesAsync();
        }

        public int Count()
        {
            var count = this._postRepository.All().Count();

            return count;
        }

        public async Task<IServiceResult> Edit(PostViewModel model)
        {
            var result = new ServiceResult
            {
                Success = false
            };

            if (model.Id == null ||
                model.Content == null ||
                model.Title == null)
            {
                result.Error = "Invalid post data!";
                return result;
            }

            var post = await this._postRepository.GetByIdAsync(model.Id);

            if (post == null)
            {
                result.Error = "Invalid post id!";
                return result;
            }

            post.Title = model.Title;
            post.Content = model.Content;
            post.ModifiedOn = DateTime.UtcNow;

            var image = this._galleryService.Upload(model.UploadImages);

            if (image != null)
            {
                post.Image = image;
            }

            try
            {
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
    }
}