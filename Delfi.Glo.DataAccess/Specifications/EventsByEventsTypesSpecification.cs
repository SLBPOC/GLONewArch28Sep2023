using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.DataAccess.Specifications
{
    public class EventsByEventsTypesSpecification : Specification<EventsDto>
    {
        public readonly EventListFilterDto _eventListFilter;
        public EventsByEventsTypesSpecification(EventListFilterDto eventListFilter)
        {
            this._eventListFilter = eventListFilter;
        }
        public override Expression<Func<EventsDto, bool>> ToExpression()
        {
            return a => _eventListFilter.EventTypes != null && _eventListFilter.EventTypes.Any(b => b == a.EventType);
        }
    }
}
