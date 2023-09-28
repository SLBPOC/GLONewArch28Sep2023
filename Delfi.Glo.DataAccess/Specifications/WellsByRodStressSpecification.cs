using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByRodStressSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByRodStressSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            int start = _wellListFilter.RodStress != null ? _wellListFilter.RodStress.Start : 0;
            int end = _wellListFilter.RodStress != null ? _wellListFilter.RodStress.End : 0;
            return a => a.RodStress != null && (a.RodStress.Value >= start && a.RodStress.Value <= end);
        }
    }
}
