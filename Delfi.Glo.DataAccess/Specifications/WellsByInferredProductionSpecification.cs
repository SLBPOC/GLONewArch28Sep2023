using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;
namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByInferredProductionSpecification : Specification<WellDto>
    {
        public readonly WellListFilterDto _wellListFilter;
        public WellsByInferredProductionSpecification(WellListFilterDto wellListFilter)
        {
            this._wellListFilter = wellListFilter;
        }
        public override Expression<Func<WellDto, bool>> ToExpression()
        {
            bool var = false;
            return a => a.InferredProduction != null && _wellListFilter.InferredProduction != null ? (a.InferredProduction.Value >= _wellListFilter.InferredProduction.Start
                && a.InferredProduction.Value <= _wellListFilter.InferredProduction.End) : var;
        }

    }
}
