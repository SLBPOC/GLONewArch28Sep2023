using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delfi.Glo.Entities.Dto;

namespace Delfi.Glo.Repository
{
    public interface IEventService<T> where T : class
    {
        Task<T> GetEventListByFilters(EventListFilterDto eventListFilter);
        Task<EventsDropDownDto> GetAllWellsAndEventTypes();
    }
}
