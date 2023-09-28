using System.Linq.Expressions;

namespace Delfi.Glo.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        T? FirstOrDefault(Expression<Func<T, bool>> expression);
        IQueryable<T> Where(Expression<Func<T, bool>> expression);
        T First();
        T Last();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}