using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByWellNamesSpecification : Specification<WellDto>
    {
        public readonly WellListFilterDto _wellListFilter;
        private readonly bool result = false;
        public WellsByWellNamesSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            return a => _wellListFilter.WellNames != null ? _wellListFilter.WellNames.Any(b => b == a.WellName) : result;
        }
    }
}
