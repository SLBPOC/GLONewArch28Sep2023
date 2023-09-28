using Ardalis.GuardClauses;
using Delfi.Glo.Api.Exceptions;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Delfi.Glo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellInfoController : ControllerBase
    {
        private readonly ILogger<WellInfoController> _logger;
        private readonly IWellInfoService<WellInfoDto> _wellInfoService;
        public WellInfoController(ILogger<WellInfoController> logger, IWellInfoService<WellInfoDto> wellInfoService)
        {
            _logger = logger;
            _wellInfoService = wellInfoService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult> Get(string WellId)
        {
            Guard.Against.InvalidString(WellId);
            var result = await _wellInfoService.GetWellInfoFromJsonFile(WellId);

            if (result != null) return Ok(result);
            else return NotFound($"No well found.");
        }
    }
}
