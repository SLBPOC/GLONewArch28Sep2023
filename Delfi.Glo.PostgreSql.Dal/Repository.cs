#pragma warning disable
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Delfi.Glo.Repository;

namespace Delfi.Glo.PostgreSql.Dal
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationContext _dbContext;
        private DbSet<T> _entity;

        public Repository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _entity = _dbContext.Set<T>();
        }

        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return _entity.FirstOrDefaultAsync(expression);
        }

        public T? FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return _entity.FirstOrDefault(expression);
        }

        public IQueryable<T> GetAll()
        {
            return _entity.AsQueryable().AsNoTracking();
        }

        public T First()
        {
            return _entity.First();
        }

        public T Last()
        {
            return _entity.Last();
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
        {
            return _entity.AnyAsync(expression);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return _entity.Where(expression).AsNoTracking();
        }


        public void Create(T entity)
        {
            _dbContext.Add(entity);
        }

        public void Update(T entity)
        {
            _entity.Update(entity);
        }

        public void Delete(T entity)
        {
            _entity.Remove(entity);
        }
    }
}
#pragma warning restore 