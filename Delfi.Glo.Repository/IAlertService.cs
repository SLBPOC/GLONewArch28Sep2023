using Delfi.Glo.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.Repository
{
    public interface IAlertService<T> where T : class
    {
        Task<bool> GetSnoozeByAlert(int? alertId, int snoozeBy);
        Task<bool> SetClearAlert(int? alertId, string comment);
        Task<T> GetAlertListByFilters(AlertListFilterDto alertListFilter);
        Task<DefaultAlertFilterValues> GetDefaultValuesForAlerts();
        Task<IEnumerable<SnoozList>> GetSnoozlistByWell(string? WellName);
        Task<bool> UpdateSnooze(int? alertId, int snoozeBy);



    }
}

