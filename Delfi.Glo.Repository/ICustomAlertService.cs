using Delfi.Glo.Entities.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Delfi.Glo.Repository
{
    public interface ICustomAlertService<T> where T : class
    {
        Task<T> GetCustomAlerts(CustomAlertFilter customAlertFilter);
        Task<bool> CreateCustomAlert(CustomAlertDto alertCustom);
        Task<bool> DeleteCustomAlert(int? id);
        Task<bool> UpdateCustomAlert(int? id, bool check);
        Task<bool> UpdateAlert(CustomAlertDto alertCustom);

    }
}