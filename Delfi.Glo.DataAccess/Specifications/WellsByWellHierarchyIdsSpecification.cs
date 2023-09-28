using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByWellHierarchyIdsSpecification : Specification<WellDto>
    {
        public readonly WellListFilterDto _wellListFilter;
        public WellsByWellHierarchyIdsSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }

        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return x => _wellListFilter.Ids != null ? _wellListFilter.Ids.Contains(x.Id) : var;
        }
    }
}
