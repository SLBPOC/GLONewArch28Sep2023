using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByGearboxLoadSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByGearboxLoadSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            int start = _wellListFilter.GearboxLoad != null ? _wellListFilter.GearboxLoad.Start : 0;
            int end = _wellListFilter.GearboxLoad != null ? _wellListFilter.GearboxLoad.End : 0;
            return a => a.GearboxLoad != null && (a.GearboxLoad.Value >= start && a.GearboxLoad.Value <= end);
        }
    }
}
