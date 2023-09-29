using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByModeOfOperationSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        private readonly bool  result=false;

        public WellsByModeOfOperationSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => _wellListFilter.ModeOfOperation != null ? _wellListFilter.ModeOfOperation.Any(b => b == a.ApprovalMode) : var;
        }
    }
}
