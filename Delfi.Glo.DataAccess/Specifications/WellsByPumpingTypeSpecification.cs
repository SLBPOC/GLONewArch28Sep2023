using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByPumpingTypeSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;

        public WellsByPumpingTypeSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => _wellListFilter.PumpingType != null && _wellListFilter.PumpingType.Any(b => b == a.WellStatus);
        }
    }
}
