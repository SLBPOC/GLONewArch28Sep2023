using Ardalis.GuardClauses;
using Delfi.Glo.Api.Exceptions;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Delfi.Glo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomAlertController : ControllerBase
    {
        private readonly ILogger<CustomAlertController> _logger;
        private readonly ICustomAlertService<CustomAlertDetailsDto> _customalertService;
        public CustomAlertController(ILogger<CustomAlertController> logger, ICustomAlertService<CustomAlertDetailsDto> customalertService)
        {
            _logger = logger;
            _customalertService = customalertService;
        }
        [HttpPost()]
        public async Task<ActionResult> Get(CustomAlertFilter customAlertFilter)
        {
            Guard.Against.InvalidPageIndex(customAlertFilter.PageNumber);
            Guard.Against.InvalidPageSize(customAlertFilter.PageSize);
            return Ok(await _customalertService.GetCustomAlerts(customAlertFilter));
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Post(CustomAlertDto customAlert)
        {
            if (customAlert == null)
                return BadRequest();

            Guard.Against.InvalidString(customAlert.CustomAlertName ?? "");
            Guard.Against.InvalidString(customAlert.Operator ?? "");
            Guard.Against.InvalidString(customAlert.Value ?? "");
            Guard.Against.InvalidString(customAlert.Category ?? "");
            Guard.Against.InvalidString(customAlert.StartDate ?? "");
            Guard.Against.InvalidString(customAlert.EndDate ?? "");
            Guard.Against.InvalidString(customAlert.NotificationType ?? "");
            Guard.Against.InvalidString(customAlert.Priority ?? "");

            return Ok(await _customalertService.CreateCustomAlert(customAlert));
        }
        [HttpDelete()]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            Guard.Against.InvalidInteger(id);
            return await _customalertService.DeleteCustomAlert(id);
        }

        [HttpPut()]
        public async Task<ActionResult<bool>> Put(int id, bool IsActive)
        {
            Guard.Against.InvalidInteger(id);
            return await _customalertService.UpdateCustomAlert(id, IsActive);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<bool>> Update(CustomAlertDto customAlert)
        {
            Guard.Against.InvalidInteger(customAlert.Id);
            return await _customalertService.UpdateAlert(customAlert);
        }
    }
}
