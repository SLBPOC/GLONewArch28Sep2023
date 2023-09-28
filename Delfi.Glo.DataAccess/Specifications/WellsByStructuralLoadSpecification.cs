using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByStructuralLoadSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByStructuralLoadSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            int start = _wellListFilter.StructuralLoad != null ? _wellListFilter.StructuralLoad.Start : 0;
            int end = _wellListFilter.StructuralLoad != null ? _wellListFilter.StructuralLoad.End : 0;
            return a => a.StructuralLoad != null && (a.StructuralLoad.Value >= start && a.StructuralLoad.Value <= end);
        }
    }
}
