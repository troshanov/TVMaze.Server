using TVMaze.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TVMaze.Data
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public EfRepository(DbContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<TEntity>();
        }
        protected DbSet<TEntity> DbSet { get; set; }

        protected DbContext Context { get; set; }

        public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

        public virtual IQueryable<TEntity> All() => this.DbSet;

        public virtual Task AddAsync(TEntity entity) => this.DbSet.AddAsync(entity).AsTask();

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();
    }
}
