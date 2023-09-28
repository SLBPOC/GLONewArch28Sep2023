using Ardalis.GuardClauses;
using Delfi.Glo.Api.Exceptions;
using Delfi.Glo.Common.Constants;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Xml;

namespace Delfi.Glo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellController : ControllerBase
    {
        private readonly ILogger<WellController> _logger;
        private readonly IWellService<WellDetailsDto> _wellService;
        private readonly IWellHierarchyService _wellHierarchyService;


        public WellController(ILogger<WellController> logger, IWellService<WellDetailsDto> wellService,IWellHierarchyService wellHierarchyService)
        {
            _logger = logger;
            _wellService = wellService;
            _wellHierarchyService = wellHierarchyService;
        }

        [HttpPost("Get")]
        public async Task<ActionResult> Get(WellListFilterDto wellListFilter)
        {
            Guard.Against.InvalidPageIndex(wellListFilter.PageNumber);
            Guard.Against.InvalidPageSize(wellListFilter.PageSize);
            var result = await _wellService.GetWellListByFilters(wellListFilter);
            if (result != null && result.WellDtos != null && result.WellDtos.Count() > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"No well found.");
            }
        }
        [HttpPost("GetDefaultValues")]
        public async Task<ActionResult> GetDefaultValues()
        {
            var result = await _wellService.GetWellFilterDefaultValues();

            if (result != null)
                return Ok(result);
            else 
                return NotFound($"No well found.");
        }

        [HttpGet("hierarchy")]
        public async Task<ActionResult> Get([FromQuery] WellHierarchyRequest? request = null)
        {
            return Ok(
                request != null && !string.IsNullOrEmpty(request.SearchText) ?
               await _wellHierarchyService.SearchInWellHierarchy(request) :
               await _wellHierarchyService.GetWellsWithHierarchy());
        }
    }
}
