using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsBySpmSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;

        public WellsBySpmSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            int start = _wellListFilter.SPM != null ? _wellListFilter.SPM.Start : 0;
            int end = _wellListFilter.SPM != null ? _wellListFilter.SPM.End : 0;
            return a => a.SPM != null && (a.SPM.Value >= start && a.SPM.Value <= end);
        }
    }
}
