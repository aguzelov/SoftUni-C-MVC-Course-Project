using Dialog.Data.Models;
using Dialog.Data.Models.Blog;
using Dialog.Data.Models.Chat;
using Dialog.Data.Models.Gallery;
using Dialog.Data.Models.News;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dialog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<ChatLine> ChatLines { get; set; }
        public virtual DbSet<UserChat> UserChats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Chat>()
                .HasMany(c => c.UserChats)
                .WithOne(uc => uc.Chat)
                .HasForeignKey(uc => uc.ChatId);

            builder.Entity<ApplicationUser>()
                .HasMany(c => c.UserChats)
                .WithOne(uc => uc.ApplicationUser)
                .HasForeignKey(uc => uc.ApplicationUserId);

            base.OnModelCreating(builder);
        }
    }
}