using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByCurrentCycleStatusSpecification : Specification<WellDto>
    {
        public readonly WellListFilterDto _wellListFilter;
        public WellsByCurrentCycleStatusSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => _wellListFilter.CurrentCycleStatus != null ? _wellListFilter.CurrentCycleStatus.Any(b => b == a.CurrentCycleStatus) : var;
        }

    }
}
