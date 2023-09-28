using Ardalis.GuardClauses;
using Delfi.Glo.Api.Exceptions;
using Delfi.Glo.Entities.Db;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Delfi.Glo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IEventService<EventDetailsDto> _eventService;
        public EventController(IEventService<EventDetailsDto> eventService)
        {
            _eventService = eventService;
        }
        #region Event
        [HttpPost("Get")]
        public async Task<ActionResult> Get(EventListFilterDto eventlistfiler)
        {
            Guard.Against.InvalidPageIndex(eventlistfiler.PageNumber);
            Guard.Against.InvalidPageSize(eventlistfiler.PageSize);
            var result = await _eventService.GetEventListByFilters(eventlistfiler);
            if (result != null && result.Events != null && result.Events.Count() > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"No well found.");
            }

        }
        [HttpPost("GetDefaultValues")]
        public async Task<ActionResult> Get()
        {
            var result = await _eventService.GetAllWellsAndEventTypes();
            if (result != null && result.WellNames != null && result.WellNames.Count() > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"No well found.");
           }
        }


        #endregion
    }
    }

