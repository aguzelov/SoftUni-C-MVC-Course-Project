using System;
using System.Collections.Generic;
using System.Linq;
using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Services.Contracts;

namespace Dialog.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> _useRepository;

        public UserService(IRepository<ApplicationUser> useRepository)
        {
            this._useRepository = useRepository;
        }

        public ICollection<T> AuthorPosts<T>()
        {
            var authorPosts = this._useRepository.All()
                .OrderByDescending(a => a.Posts.Count)
                .To<T>()
                .ToList();

            return authorPosts;
        }
    }
}