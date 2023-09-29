using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByModeOfApprovalSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;

        public WellsByModeOfApprovalSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => _wellListFilter.ModeOfApproval != null ? _wellListFilter.ModeOfApproval.Any(b => b == a.ApprovalMode) : var;
        }
    }
}
