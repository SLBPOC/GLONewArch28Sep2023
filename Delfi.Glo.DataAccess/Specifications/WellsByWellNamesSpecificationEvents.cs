using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class WellsByWellNamesSpecificationEvents : Specification<EventsDto>
    {
        public readonly EventListFilterDto _eventListFilter;
        public WellsByWellNamesSpecificationEvents(EventListFilterDto eventListFilter)
        {
            this._eventListFilter = eventListFilter;
        }
        public override Expression<Func<EventsDto, bool>> ToExpression()
        {
            return a => _eventListFilter.WellNames != null && _eventListFilter.WellNames.Any(b => b == a.WellName);
        }
    }
}
