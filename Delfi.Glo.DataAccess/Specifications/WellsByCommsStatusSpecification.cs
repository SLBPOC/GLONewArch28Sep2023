using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByCommsStatusSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;


        public WellsByCommsStatusSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => _wellListFilter.CommStatus != null ? _wellListFilter.CommStatus.Any(b => b == a.CommStatus) : var;
        }
    }
}
