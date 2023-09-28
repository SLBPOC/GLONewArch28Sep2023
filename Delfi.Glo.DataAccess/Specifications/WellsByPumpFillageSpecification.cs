using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByPumpFillageSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        private readonly bool  result=false;

        public WellsByPumpFillageSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => a.PumpFillage != null && _wellListFilter.PumpFillage != null ? (a.PumpFillage.Value >= _wellListFilter.PumpFillage.Start
                && a.PumpFillage.Value <= _wellListFilter.PumpFillage.End) : result;
        }
    }
}
