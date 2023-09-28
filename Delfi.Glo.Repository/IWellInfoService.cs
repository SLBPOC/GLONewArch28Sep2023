
namespace Delfi.Glo.Repository
{
    public interface IWellInfoService<T> where T : class
    {
        Task<T?> GetWellInfoFromJsonFile(string WellId);
    }
}
