using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByProductionUpTimeSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByProductionUpTimeSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => _wellListFilter.ProductionUpTime != null ? _wellListFilter.ProductionUpTime.Any(b => b == a.ProductionUpTime) : var;
        }
    }
}
