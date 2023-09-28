using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByCyclesTodaySpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByCyclesTodaySpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            int start = _wellListFilter.CyclesToday != null ? _wellListFilter.CyclesToday.Start : 0;
            int end = _wellListFilter.CyclesToday != null ? _wellListFilter.CyclesToday.End : 0;
            return a => a.CyclesToday != null && (a.CyclesToday.Value >= start && a.CyclesToday.Value <= end);
        }
    }
}
