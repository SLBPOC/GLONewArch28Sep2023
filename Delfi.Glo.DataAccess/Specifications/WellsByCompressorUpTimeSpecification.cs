using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByCompressorUpTimeSpecification : Specification<WellDto>
    {
        private readonly WellListFilterDto _wellListFilter;
        public WellsByCompressorUpTimeSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => _wellListFilter.CompressorUpTime != null ? _wellListFilter.CompressorUpTime.Any(b => b == a.CompressorUpTime) : var;
        }
    }
}
