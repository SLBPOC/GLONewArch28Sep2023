using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByLastCycleStatusSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;


        public WellsByLastCycleStatusSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => _wellListFilter.LastCycleStatus != null ? _wellListFilter.LastCycleStatus.Any(b => b == a.LastCycleStatus) : var;
        }
    }
}
