using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByControllerStatusSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;

        public WellsByControllerStatusSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => _wellListFilter.ControllerStatus != null ? _wellListFilter.ControllerStatus.Any(b => b == a.ControllerStatus) : false;
        }
    }
}
