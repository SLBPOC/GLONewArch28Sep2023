using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System.Linq.Expressions;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class EventsByWellHierarchyIdsSpecification : Specification<EventsDto>
    {
        public readonly EventListFilterDto _eventListFilter;
        public EventsByWellHierarchyIdsSpecification(EventListFilterDto eventListFilter)
        {
            this._eventListFilter = eventListFilter;
        }

        public override Expression<Func<EventsDto, bool>> ToExpression()
        {
            return x => _eventListFilter.Ids != null && _eventListFilter.Ids.Contains(Convert.ToInt32(x.WellId));
        }
    }
}
