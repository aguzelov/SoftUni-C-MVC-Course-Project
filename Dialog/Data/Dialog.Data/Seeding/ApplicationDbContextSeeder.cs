using Dialog.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;
using Dialog.Data.Models.News;

namespace Dialog.Data.Seeding
{
    public static class ApplicationDbContextSeeder
    {
        public static void Seed(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Seed(dbContext, roleManager, userManager);
        }

        public static void Seed(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }

            if (userManager == null)
            {
                throw new ArgumentException(nameof(userManager));
            }

            SeedRoles(roleManager);

            SeedPosts(dbContext, userManager);

            SeedNews(dbContext, userManager);
        }

        private static void SeedNews(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            if (!dbContext.News.Any())
            {
                var allNews = new List<News>();

                var random = new Random();
                var users = userManager.Users.ToList();

                for (int i = 0; i < 30; i++)
                {
                    var user = users[random.Next(0, users.Count())];

                    var news = new News
                    {
                        Title = LoremIpsum(1, 5, 1, 1, 1),
                        Content = LoremIpsum(5, 10, 4, 10, 2),
                        CreatedOn = DateTime.UtcNow.AddDays(random.Next(0, 40) * -1),
                        Author = user,
                    };

                    allNews.Add(news);
                }

                dbContext.News.AddRange(allNews);
                dbContext.SaveChanges();
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            SeedRole(GlobalConstants.AdminRole, roleManager);
            SeedRole(GlobalConstants.UserRole, roleManager);
        }

        private static void SeedRole(string roleName, RoleManager<IdentityRole> roleManager)
        {
            var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
            if (role == null)
            {
                var result = roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static void SeedPosts(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            if (!dbContext.Posts.Any())
            {
                var posts = new List<Post>();

                var random = new Random();
                var users = userManager.Users.ToList();

                for (int i = 0; i < 30; i++)
                {
                    var user = users[random.Next(0, users.Count())];

                    var post = new Post
                    {
                        Title = LoremIpsum(1, 5, 1, 1, 1),
                        Content = LoremIpsum(5, 10, 4, 10, 2),
                        CreatedOn = DateTime.UtcNow.AddDays(random.Next(0, 40) * -1),
                        Author = user,
                        Comments = GetCommments(dbContext, user.UserName, random.Next(1, 10)),
                    };

                    posts.Add(post);
                }

                dbContext.AddRange(posts);
                dbContext.SaveChanges();
            }
        }

        private static ICollection<Comment> GetCommments(ApplicationDbContext dbContext, string authorName, int commentCount)
        {
            var comments = new List<Comment>();

            var random = new Random();

            for (int i = 0; i < commentCount; i++)
            {
                var comment = new Comment
                {
                    Author = authorName,
                    Content = LoremIpsum(4, 10, 2, 4, 1),
                    CreatedOn = DateTime.UtcNow.AddDays(random.Next(0, 40) * -1),
                    Replies = GetReplies(authorName, random.Next(0, 3)),
                };
            }

            dbContext.Comments.AddRange(comments);

            return comments;
        }

        private static ICollection<Comment> GetReplies(string authorName, int commentCount)
        {
            var replies = new List<Comment>();

            var random = new Random();

            for (int i = 0; i < commentCount; i++)
            {
                var comment = new Comment
                {
                    Author = authorName,
                    Content = LoremIpsum(4, 10, 2, 4, 1),
                    CreatedOn = DateTime.UtcNow.AddDays(random.Next(0, 40) * -1),
                };
            }

            return replies;
        }

        private static string LoremIpsum(int minWords, int maxWords,
                                int minSentences, int maxSentences,
                                int numParagraphs)
        {
            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        result.Append(words[rand.Next(words.Length)]);
                    }
                    result.Append(". ");
                }
            }

            return result.ToString();
        }
    }
}