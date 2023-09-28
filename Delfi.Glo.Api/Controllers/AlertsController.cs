using Ardalis.GuardClauses;
using Delfi.Glo.Api.Exceptions;
using Delfi.Glo.Common.Constants;
using Delfi.Glo.Entities.Db;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Delfi.Glo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : Controller
    {
        
        private readonly IAlertService<AlertsDetailsDto> _alertService;
        public AlertsController(IAlertService<AlertsDetailsDto> alertService)
        {           
            _alertService = alertService;
        }
        #region Alert
        [HttpPost("Get")]
        public async Task<ActionResult> Get(AlertListFilterDto alertListFilter)
        {
            Guard.Against.InvalidPageIndex(alertListFilter.PageNumber);
            Guard.Against.InvalidPageSize(alertListFilter.PageSize);
            var result = await _alertService.GetAlertListByFilters(alertListFilter);
            if (result != null && result.Alerts != null && result.Alerts.Count() > 0)
            {
                return Ok(result);
            }
            else
            {
                return NotFound($"No well found.");
            }
        }
        [HttpPost("GetSnoozbyWells")]
        public async Task<ActionResult> GetSnoozbyWells(string WellName)
        {
            Guard.Against.InvalidString(WellName);
            var result= await _alertService.GetSnoozlistByWell(WellName);
            if (result != null && result?.Count() > 0)
                return Ok(result);
            else
                return NotFound($"No Snooz  found.");

        }
        [HttpPost("SnoozeBy")]
        public async Task<bool> GetSnoozeByAlert(int alertId, int snoozeBy)
        {
            Guard.Against.InvalidInteger(alertId);
            Guard.Against.InvalidInteger(snoozeBy);
            return await _alertService.GetSnoozeByAlert(alertId, snoozeBy);
        }
        [HttpPost("ClearAlert")]
        public async Task<bool> SetClearAlert(int alertId, string comment)
        {
            Guard.Against.InvalidInteger(alertId);
            Guard.Against.InvalidString(comment);
            return await _alertService.SetClearAlert(alertId, comment);
        }

        [HttpPost("GetDefaultValues")]
        public async Task<ActionResult> GetDefaultValues()
        {
            var result = await _alertService.GetDefaultValuesForAlerts();

            if (result != null)
                return Ok(result);
            else
                return NotFound($"No well found.");
        }
        [HttpPost("ClearSnooze")]
        public async Task<bool> UpdateSnooze(int alertId, int snoozeBy)
        {
            Guard.Against.InvalidInteger(alertId);
            Guard.Against.InvalidInteger(snoozeBy);
            return await _alertService.UpdateSnooze(alertId, snoozeBy);
        }

        #endregion
    }
}
