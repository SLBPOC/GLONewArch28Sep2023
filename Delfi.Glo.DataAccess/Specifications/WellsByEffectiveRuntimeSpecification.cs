using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByEffectiveRuntimeSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByEffectiveRuntimeSpecification(WellListFilterDto wellListFilter) 
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            int start = _wellListFilter.EffectiveRuntime != null ? _wellListFilter.EffectiveRuntime.Start : 0;
            int end = _wellListFilter.EffectiveRuntime != null ? _wellListFilter.EffectiveRuntime.End : 0;
            return a => a.EffectiveRunTime != null && (a.EffectiveRunTime.Value >= start && a.EffectiveRunTime.Value <= end);
        }
    }
}
