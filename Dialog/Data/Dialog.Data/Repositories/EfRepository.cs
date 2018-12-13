using Dialog.Data.Common.Models;

namespace Dialog.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Dialog.Data.Common.Repositories;

    using Microsoft.EntityFrameworkCore;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EfRepository(ApplicationDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => this.DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

        public IQueryable<TEntity> AllWithoutDeleted()
        {
            if (typeof(IDeletableEntity).IsAssignableFrom(typeof(TEntity)))
            {
                return this.DbSet.Where(t => !(t as IDeletableEntity).IsDeleted);
            }

            return this.DbSet;
        }

        public virtual Task<TEntity> GetByIdAsync(params object[] id) => this.DbSet.FindAsync(id);

        public virtual void Add(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            this.DbSet.Remove(entity);
        }

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

        public void Dispose() => this.Context.Dispose();
    }
}