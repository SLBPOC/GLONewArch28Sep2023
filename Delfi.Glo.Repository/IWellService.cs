using Delfi.Glo.Entities.Dto;

namespace Delfi.Glo.Repository
{
    public interface IWellService<T> where T : class
    {
        Task<T> GetWellListByFilters(WellListFilterDto wellListFilter);
        Task<DefaultWellFilterValues> GetWellFilterDefaultValues();
        Task<IQueryable<WellDto>?> GetWells();
    }
}
