using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dialog.Common;

namespace Dialog.Services
{
    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IDeletableEntityRepository<ApplicationUser> userRepository, UserManager<ApplicationUser> userManager)
        {
            this._userRepository = userRepository;
            this._userManager = userManager;
        }

        public ICollection<T> All<T>()
        {
            var users = this._userRepository.All()
                .OrderByDescending(u => u.CreatedOn)
                .To<T>()
                .ToList();

            return users;
        }

        public ICollection<T> AuthorWithPostsCount<T>()
        {
            var authorPosts = this._userRepository.All()
                .OrderByDescending(a => a.Posts.Count)
                .To<T>()
                .ToList();

            return authorPosts;
        }

        public ICollection<T> AuthorWithNewsCount<T>()
        {
            var authorPosts = this._userRepository.All()
                .OrderByDescending(a => a.News.Count)
                .To<T>()
                .ToList();

            return authorPosts;
        }

        public int Count()
        {
            var count = this._userRepository.All().Count();

            return count;
        }

        public async Task<string> GetUserRoles(string email)
        {
            var user = await this._userManager.FindByEmailAsync(email);

            var roles = await this._userManager.GetRolesAsync(user);

            return string.Join("; ", roles);
        }

        public async Task Approve(string id)
        {
            var user = await this._userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return;
            }

            user.IsApproved = true;
            this._userRepository.Update(user);
            await this._userRepository.SaveChangesAsync();
        }

        public async Task Promote(string id)
        {
            var user = await this._userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return;
            }

            await this._userManager.AddToRoleAsync(user, GlobalConstants.AdminRole);
        }

        public async Task Demote(string id)
        {
            var user = await this._userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return;
            }

            await this._userManager.RemoveFromRoleAsync(user, GlobalConstants.AdminRole);
        }

        public async Task DeleteUser(string id)
        {
            var user = await this._userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return;
            }

            this._userRepository.Delete(user);
            await this._userRepository.SaveChangesAsync();
        }
    }
}