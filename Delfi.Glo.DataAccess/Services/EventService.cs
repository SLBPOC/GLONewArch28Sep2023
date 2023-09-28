using Delfi.Glo.Common;
using Delfi.Glo.Common.Constants;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Repository;
using Delfi.Glo.DataAccess.Specifications;
using System.Net.Http.Headers;

namespace Delfi.Glo.DataAccess.Services
{
    public class EventService : IEventService<EventDetailsDto>
    {
        #region Public methods


        ///<summary>
        ///Get Default values of filter for well and event types
        ///</summary>
 
        public async Task<EventsDropDownDto> GetAllWellsAndEventTypes()
        {
            var wellsJson = (await UtilityService.ReadAsync<List<WellDto>>(JsonFiles.WELLS))?.AsQueryable();

            EventsDropDownDto EventsDropDownDto = new();
            if (wellsJson != null)
            {
                var wells = wellsJson.Select(a => new WellIdName { WellId = a.WellId, WellName = a.WellName }).DistinctBy(a => a.WellName).ToList();
                EventsDropDownDto.WellNames = wells;
            }
            EventTypes objEventTypes = new();
            EventsDropDownDto.EventTypes = objEventTypes.EventTypeList.ToList();
            return EventsDropDownDto;
        }


        ///<summary>
        ///Get Event list based on filter values
        /// Input EventListFilter Object: Filter object which contains pagination , sorting , searching , well names, event types , time range details to filter
        ///</summary>
        public async Task<EventDetailsDto> GetEventListByFilters(EventListFilterDto eventListFilter)
        {
            var eventsJson = (await UtilityService.ReadAsync<List<EventsDto>>(JsonFiles.EVENTS))?.AsQueryable(); 
            EventDetailsDto EventsDetailsDto = new();
            List<EventsDto> eventsList = new();
            if (eventsJson != null)
            {
                eventsJson = GetWellsByWellNames(eventListFilter, eventsJson);
                eventsJson = GetWellsByEventTypes(eventListFilter, eventsJson);
                eventsJson = DateRange(eventListFilter.DateRange != null ? eventListFilter.DateRange.FromDate ?? "" : "", eventListFilter.DateRange != null ? eventListFilter.DateRange.ToDate ?? "" : "", eventsJson);
                eventsJson = SearchText(eventListFilter.SearchText ?? "", eventsJson);
                eventsJson = GetEventsByWellHierarchyIds(eventListFilter, eventsJson);
                eventsJson = Sort(eventListFilter.SortColumn ?? "", eventListFilter.SortDirection ?? "", eventsJson);
                eventsList = eventsJson.Skip((eventListFilter.PageNumber - 1) * eventListFilter.PageSize).Take(eventListFilter.PageSize).ToList();
                EventsDetailsDto.Totalcount = eventsJson.Count();
            }
            EventsDetailsDto.Events = (List<EventsDto>?)eventsList;
            return EventsDetailsDto;
        }
        #endregion


        #region Private Methods

        ///<summary>
        ///Get Event list after well filter
        /// Input EventListFilter Object: Filter object which contains pagination , sorting , searching , well names, event types , time range details to filter
        /// IQueryable<EventsDto> Object : list to filter details
        ///</summary>
        private static IQueryable<EventsDto> GetWellsByWellNames(EventListFilterDto eventListFilter, IQueryable<EventsDto> eventsInJson)
        {
            var searchevents = eventsInJson;
            if (eventListFilter.WellNames != null && eventListFilter.WellNames.Any())
            {
                var StatusSpecification = new WellsByWellNamesSpecificationEvents(eventListFilter);
                searchevents = searchevents.Where(StatusSpecification.ToExpression());
                eventsInJson = searchevents;
            }
            return eventsInJson;
        }


        ///<summary>
        ///Get Event list after event type filter
        /// Input EventListFilter Object: Filter object which contains pagination , sorting , searching , well names, event types , time range details to filter
        /// IQueryable<EventsDto> Object : list to filter details
        ///</summary>
        private static IQueryable<EventsDto> GetWellsByEventTypes(EventListFilterDto eventListFilter, IQueryable<EventsDto> eventsInJson)
        {
            var searchevents = eventsInJson;
            if (eventListFilter.EventTypes != null && eventListFilter.EventTypes.Any())
            {
                var StatusSpecification = new EventsByEventsTypesSpecification(eventListFilter);
                searchevents = searchevents.Where(StatusSpecification.ToExpression());
                eventsInJson = searchevents;
            }
            return eventsInJson;
        }

        ///<summary>
        ///Get Event list after date range filter
        /// FromDate string: Filter from date
        /// ToDate string : filter to date
        /// IQueryable<EventsDto> Object : list to filter details
        ///</summary>
        private static IQueryable<EventsDto> DateRange(string FromDate, string ToDate, IQueryable<EventsDto> alertsList)
        {
            var alerts = alertsList.AsQueryable();
            if (FromDate != null && ToDate != null && FromDate != "" && ToDate != "")
            {
                var dateRangeSpecification = new EventbyDateSpecification(FromDate, ToDate);
                alerts = alerts.Where(dateRangeSpecification.ToExpression());
            }
            return alerts;
        }

        ///<summary>
        ///Get Event list after search text filter
        /// SearchText string: Filter by text        
        /// IQueryable<EventsDto> Object : list to filter details
        ///</summary>
        private static IQueryable<EventsDto> SearchText(string SearchText, IQueryable<EventsDto> alertsList)
        {
            var alerts = alertsList.AsQueryable();
            if (!String.IsNullOrEmpty(SearchText))
            {
                var searchTextSpecification = new EventbySearchTextSpecification(SearchText.ToLower());
                alerts = alerts.Where(searchTextSpecification.ToExpression());
            }
            return alerts;
        }

        ///<summary>
        ///Get Event list after sorting by specified column and direction
        /// SortColumn string: Sort By column name   
        /// SortDirection string: Sort By direction name
        /// IQueryable<EventsDto> Object : list to filter details
        ///</summary>
        private static IQueryable<EventsDto> Sort(string SortColumn, string SortDirection, IQueryable<EventsDto> alertsList)
        {
            var alerts = alertsList.AsQueryable();
            if (SortColumn != null && SortColumn != "" && SortDirection != null && SortDirection != "")
            {
                if (SortDirection == "asc")
                {
                    alerts = alerts.AsQueryable().OrderBy(SortColumn);
                }
                else
                {
                    alerts = alerts.AsQueryable().OrderByDescending(SortColumn);
                }
            }
            return alerts;
        }

        /// <summary>
        /// GetEventsByWellHierarchyIds  Get events filters after well ids from well hiearachy
        /// </summary>
        /// <param name="eventListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="eventsInJson">List to filter</param>
        /// <returns>EventsDto object contains filtered wells</returns>
        private static IQueryable<EventsDto> GetEventsByWellHierarchyIds(EventListFilterDto eventListFilter, IQueryable<EventsDto> eventsInJson)
        {
            var searchwells = eventsInJson;
            if (eventListFilter.Ids != null && eventListFilter.Ids.Any())
            {
                var StatusSpecification = new EventsByWellHierarchyIdsSpecification(eventListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                eventsInJson = searchwells;
            }
            return eventsInJson;
        }

        #endregion

    }

}
