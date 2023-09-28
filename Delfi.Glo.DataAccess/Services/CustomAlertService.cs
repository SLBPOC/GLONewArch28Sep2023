using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Repository;
using Delfi.Glo.Common.Constants;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Common;
using Delfi.Glo.DataAccess.Specifications;

namespace Delfi.Glo.PostgreSql.Dal.Services
{
    public class CustomAlertService : ICustomAlertService<CustomAlertDetailsDto>
    {
        #region Public methods

        /// <summary>
        /// Get Custom Alerts by filter
        /// </summary>
        /// <param name="customAlertFilter">Filter object which contains pagination , sorting filter details</param>
        /// <returns>CustomAlertDetailsDto object</returns>
        public async Task<CustomAlertDetailsDto> GetCustomAlerts(CustomAlertFilter customAlertFilter)
        {
            var customAlertInJson = (await UtilityService.ReadAsync<List<CustomAlertDto>>
                                                    (JsonFiles.CUSTOMALERT))?.AsQueryable();

            var wellsInJson = (await UtilityService.ReadAsync<List<WellDto>>
                                                    (JsonFiles.WELLS))?.AsQueryable();



            CustomAlertDetailsDto customAlertDetailsDto = new();
            if (customAlertInJson != null)
            {
                customAlertDetailsDto.CountDetails = new CountDetails();


                customAlertInJson = GetSortingResult(customAlertFilter, customAlertInJson);

                var finalResults = customAlertInJson.Skip((customAlertFilter.PageNumber - 1) * customAlertFilter.PageSize)
                                          .Take(customAlertFilter.PageSize)
                                          // .OrderByDescending(a => a.Id)
                                          .ToList();

                customAlertDetailsDto.CustomAlertDto = finalResults;
                customAlertDetailsDto.CountDetails.TotalCount = customAlertInJson.Count();
            }

            if (wellsInJson != null)
            {
                customAlertDetailsDto.WellFilterListDetails = new List<WellFilterListDetails>();
                var wells = wellsInJson.Select(a => new WellFilterListDetails { WellId = a.WellId, WellName = a.WellName }).ToList();
                customAlertDetailsDto.WellFilterListDetails = wells;

            }

            return customAlertDetailsDto;
        }

        /// <summary>
        /// Create Custom Alert 
        /// </summary>
        /// <param name="alertCustom">Alert object to create custom alert</param>
        /// <returns>true/false </returns>
        public async Task<bool> CreateCustomAlert(CustomAlertDto alertCustom)
        {
            var customAlertInJson = (await UtilityService.ReadAsync<List<CustomAlertDto>>
                                                    (JsonFiles.CUSTOMALERT))?.ToList();
            if (customAlertInJson != null)
            {
                List<CustomAlertDto> alertCustomList = customAlertInJson;
                alertCustomList.Add(new CustomAlertDto()
                {
                    Id = alertCustomList[^1].Id + 1,
                    WellName = alertCustom.WellName,
                    CustomAlertName = alertCustom.CustomAlertName,
                    Category = alertCustom.Category,
                    Priority = alertCustom.Priority,
                    NotificationType = alertCustom.NotificationType,
                    Operator = alertCustom.Operator,
                    Value = alertCustom.Value,
                    ActualValue = alertCustom.ActualValue,
                    IsActive = alertCustom.IsActive,
                    StartDate = alertCustom.StartDate,
                    EndDate = alertCustom.EndDate
                });
                var filePath = JsonFiles.CUSTOMALERT;
                bool data = UtilityService.Write<CustomAlertDto>(alertCustomList, filePath);
                return data;
            }
            else
                return false;
        }


        /// <summary>
        /// Delete Custom Alert by Id
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>true/false</returns>
        public async Task<bool> DeleteCustomAlert(int? id)
        {
            var customAlertInJson = (await UtilityService.ReadAsync<List<CustomAlertDto>>
                                                    (JsonFiles.CUSTOMALERT))?.AsQueryable();
            if (customAlertInJson == null) return false;

            List<CustomAlertDto> alertCustomList = customAlertInJson.ToList();
            var spec = new CustomAlertSpecification(id);
            var obj = customAlertInJson.FirstOrDefault(spec.ToExpression());
            if (obj == null)
            {
                return false;
            }
            alertCustomList.Remove(obj);
            var filePath = JsonFiles.CUSTOMALERT;
            bool data = UtilityService.Write<CustomAlertDto>(alertCustomList, filePath);
            return data;
        }


        /// <summary>
        /// Update Toggle ,Set Active/Inactive alert
        /// </summary>
        /// <param name="id">By Id</param>
        /// <param name="check">Active flag to set</param>
        /// <returns>true/false</returns>
        public async Task<bool> UpdateCustomAlert(int? id, bool check)
        {
            var customAlertInJson = (await UtilityService.ReadAsync<List<CustomAlertDto>>
                                                   (JsonFiles.CUSTOMALERT))?.AsQueryable();

            if (customAlertInJson == null) { return false; }

            List<CustomAlertDto> alertCustomList = customAlertInJson.ToList();
            var spec = new CustomAlertSpecification(id);
            var obj = customAlertInJson.FirstOrDefault(spec.ToExpression());
            if (obj == null)
            {
                return false;
            }
            obj.IsActive = check;
            var filePath = JsonFiles.CUSTOMALERT;
            return UtilityService.Write<CustomAlertDto>(alertCustomList, filePath);
        }

        /// <summary>
        /// Update Alert- Update custom alert details
        /// </summary>
        /// <param name="alertCustom">Custom alert object to update</param>
        /// <returns>true/false</returns>
        public async Task<bool> UpdateAlert(CustomAlertDto alertCustom)
        {
            var customAlertInJson = (await UtilityService.ReadAsync<List<CustomAlertDto>>
                                                   (JsonFiles.CUSTOMALERT))?.AsQueryable();

            if (customAlertInJson == null) { return false; }

            List<CustomAlertDto> alertCustomList = customAlertInJson.ToList();
            var spec = new CustomAlertSpecification(alertCustom.Id);
            var obj = customAlertInJson.FirstOrDefault(spec.ToExpression());
            if (obj == null)
            {
                return false;
            }

            obj.WellName = alertCustom.WellName;
            obj.CustomAlertName = alertCustom.CustomAlertName;
            obj.Category = alertCustom.Category;
            obj.Priority = alertCustom.Priority;
            obj.NotificationType = alertCustom.NotificationType;
            obj.Operator = alertCustom.Operator;
            obj.Value = alertCustom.Value;
            obj.ActualValue = alertCustom.ActualValue;
            obj.IsActive = alertCustom.IsActive;
            obj.StartDate = alertCustom.StartDate;
            obj.EndDate = alertCustom.EndDate;
            var filePath = JsonFiles.CUSTOMALERT;
            return UtilityService.Write<CustomAlertDto>(alertCustomList, filePath);
        }


        #endregion



        #region Private methods

        /// <summary>
        /// Get Sorting Result
        /// </summary>
        /// <param name="customAlertFilter">Sorting column and direction</param>
        /// <param name="customAlertInJson">list to sort</param>
        /// <returns>IQueryable<CustomAlertDto> Sorted result</returns>
        private static IQueryable<CustomAlertDto> GetSortingResult(CustomAlertFilter customAlertFilter, IQueryable<CustomAlertDto> customAlertInJson)
        {
            var searchAlerts = customAlertInJson;
            if (customAlertFilter.SortColumn != null && customAlertFilter.SortColumn != "" && customAlertFilter.SortDirection != null && customAlertFilter.SortDirection != "")
            {
                if (customAlertFilter.SortDirection == "asc")
                {
                    customAlertInJson = searchAlerts.AsQueryable().OrderBy(customAlertFilter.SortColumn);
                }
                else
                {
                    customAlertInJson = searchAlerts.AsQueryable().OrderByDescending(customAlertFilter.SortColumn);
                }
            }

            return customAlertInJson;
        }

        #endregion






        


    }
}