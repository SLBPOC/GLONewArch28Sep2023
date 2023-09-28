namespace Delfi.Glo.Repository
{
    public interface ICrudService<T> where T : class
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T item);
        Task<T> UpdateAsync(int id, T item);
        Task<bool> DeleteAsync(int id);
  
        Task<bool> ExistsAsync(int id);        
     

    }
}
