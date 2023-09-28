using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByMinMaxLoadSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByMinMaxLoadSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            int start = _wellListFilter.MinMaxLoad != null ? _wellListFilter.MinMaxLoad.Start : 0;
            int end = _wellListFilter.MinMaxLoad != null ? _wellListFilter.MinMaxLoad.End : 0;
            return a => a.MinMaxLoad != null && (a.MinMaxLoad.Value >= start && a.MinMaxLoad.Value <= end);
        }
    }
}
