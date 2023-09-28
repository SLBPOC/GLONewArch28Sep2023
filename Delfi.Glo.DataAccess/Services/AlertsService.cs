
using Delfi.Glo.Common;
using Delfi.Glo.Common.Constants;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Db;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Delfi.Glo.DataAccess.Specifications;

namespace Delfi.Glo.DataAccess.Services
{
    public class AlertsService : IAlertService<AlertsDetailsDto>
    {
        #region public methods

        /// <summary>
        /// GetAlertListByFilters Get alert details by filteration
        /// </summary>
        /// <param name="alertListFilter">Filter object which contains pagination , sorting , searching , well name etc details to filter</param>
        /// <returns>AlertsDetailsDto object :list of alert along with some common details</returns>
        public async Task<AlertsDetailsDto> GetAlertListByFilters(AlertListFilterDto alertListFilter)
        {
            var alertsJson = ((await UtilityService.ReadAsync<List<AlertsDto>>(JsonFiles.ALERTS)) ?? new List<AlertsDto>()).AsQueryable();
            AlertsDetailsDto alertsDetailsDto = new();
            AlertsLevelDto alertsLevelDto = new();
            List<AlertsDto> alertsList = new();
            List<Alertcount> alertcounts = new();
            List<AlertCategory> alertCategories = new();


            if (alertsJson.Any())
            {
                alertsList = alertsJson.ToList();

                alertsList = DateRange(alertListFilter.DateRange != null ? alertListFilter.DateRange.FromDate ?? "" : "", alertListFilter.DateRange != null ? alertListFilter.DateRange.ToDate ?? "" : "", alertsList);

                alertsList = SearchText(alertListFilter.SearchText ?? "", alertsList);

                alertsList = SearchStatus(alertListFilter.SearchStatus ?? "", alertsList);
                alertsList = GetWellsByWellNames(alertListFilter, alertsList);
                alertsList = GetWellsByids(alertListFilter, alertsList);
                alertsList = GetWellbyCategorys(alertListFilter, alertsList);
                alertsList = Sort(alertListFilter.SortColumn ?? "", alertListFilter.SortDirection ?? "", alertsList);
                alertsLevelDto = AlertLevel(alertsList);
                alertcounts = Alertcountlist(alertsList);
                alertCategories = AlertCategorieslist(alertsList);
                alertsList = SnoozeByFalse(alertsList);
                alertsList = alertsList.Skip((alertListFilter.PageNumber - 1) * alertListFilter.PageSize).Take(alertListFilter.PageSize)
                    .OrderByDescending(a => a.AlertLevel).ToList();


            }
            alertsDetailsDto.Alerts = (List<AlertsDto>?)alertsList;
            alertsDetailsDto.AlertsLevelDto = alertsLevelDto;
            alertsDetailsDto.Alertcategory = (List<AlertCategory>?)alertCategories;
            alertsDetailsDto.Alertcount = (List<Alertcount>?)alertcounts;
            return alertsDetailsDto;
        }
        /// <summary>
        /// GetWells By Wellid
        /// </summary>
        /// <returns></returns>
        private List<AlertsDto> GetWellsByids(AlertListFilterDto alertListFilter, List<AlertsDto> alertsList)
        {
            var searchwells = alertsList.AsQueryable();
            if ((alertListFilter.Ids != null))
            {
                int? Wellids = alertListFilter.Ids.FirstOrDefault();
                if (Wellids > 0)
                {
                    var StatusSpecification = new AlerrtByWellidSpectification(alertListFilter);
                    searchwells = searchwells.Where(StatusSpecification.ToExpression());
                    alertsList = searchwells.ToList();
                }
            }
            return alertsList;
        }

        /// <summary>
        /// GetDefaultValues Get default filter values
        /// </summary>
        /// <returns></returns>
        public Task<DefaultAlertFilterValues> GetDefaultValuesForAlerts()
        {
            var alertInJson = UtilityService.Read<List<AlertsDto>>(JsonFiles.ALERTS)?.AsQueryable();


            DefaultAlertFilterValues defaultAlertFilterValues = new()
            {
                Category = new List<Category>()
            };
            if (alertInJson != null)
            {
                var wells = alertInJson.Select(a => new WellIdName { WellId = a.WellId, WellName = a.WellName }).DistinctBy(a => a.WellName).ToList();
                defaultAlertFilterValues.WellNames = wells;

                var Category = alertInJson.Select(a => new Category { Value = a.Category }).DistinctBy(a => a.Value).ToList();
                defaultAlertFilterValues.Category = Category;
            }



            return Task.FromResult(defaultAlertFilterValues);
        }

        /// <summary>
        /// GetSnoozlistByWell Get Snnoze list by well
        /// </summary>
        /// <param name="Wellname">Well Name</param>
        /// <returns>IEnumerable<SnoozList> object</returns>
        public async Task<IEnumerable<SnoozList>> GetSnoozlistByWell(string? WellName)
        {

            List<SnoozList> snoozLists = new();
            var alertsJson = ((await UtilityService.ReadAsync<List<AlertsDto>>(JsonFiles.ALERTS)) ?? new List<AlertsDto>()).AsQueryable();
            var Alertdto = alertsJson.Where(m => m.WellName == WellName && m.SnoozeFlag).Select(e => new
            {
                WellName = e.WellName,
                desc = e.Desc,
                alertid = e.AlertId,

            }).ToList();

            foreach (var alert in Alertdto)
            {
                SnoozList snooz = new()
                {
                    Alertid = alert.alertid,
                    Desc = alert.desc,
                    Wellname=alert.WellName
                };
                snoozLists.Add(snooz);

            }
            return snoozLists;



        }


        /// <summary>
        /// UpdateSnooz Update Snooze
        /// </summary>
        /// <param name="alertId">alertId to update snooze</param>
        /// <param name="snoozeBy">Time to snooze</param>
        /// <returns></returns>
        public async Task<bool> UpdateSnooze(int? alertId, int snoozeBy)
        {
            GetAllAlertsAndAlert(alertId, out List<AlertsDto> alertsList, out AlertsDto? obj);
            if (obj == null)
                return false;
            obj.SnoozeFlag = false;
            obj.SnoozeDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sssZ", CultureInfo.InvariantCulture);
            obj.SnoozeInterval = snoozeBy;
            bool data = await UtilityService.WriteAsync<AlertsDto>(alertsList, JsonFiles.ALERTS);
            return data;
        }

        /// <summary>
        /// GetSnoozeByAlert Get snooze alerts
        /// </summary>
        /// <param name="alertId">by AlertId</param>
        /// <param name="snoozeBy">by snoozeby time</param>
        /// <returns></returns>
        public async Task<bool> GetSnoozeByAlert(int? alertId, int snoozeBy)
        {
            GetAllAlertsAndAlert(alertId, out List<AlertsDto> alertsList, out AlertsDto? obj);
            if (obj == null)
                return false;
            obj.SnoozeFlag = true;
            obj.SnoozeDateTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sssZ", CultureInfo.InvariantCulture);
            obj.SnoozeInterval = snoozeBy;
            bool data = await UtilityService.WriteAsync<AlertsDto>(alertsList, JsonFiles.ALERTS);
            return data;
        }

        /// <summary>
        /// SetClearAlert
        /// </summary>
        /// <param name="alertId"> clear alert by alert id</param>
        /// <param name="comment">clear alert comment</param>
        /// <returns>bool value</returns>
        public async Task<bool> SetClearAlert(int? alertId, string comment)
        {
            GetAllAlertsAndAlert(alertId, out List<AlertsDto> alertsList, out AlertsDto? obj);
            if (obj == null)
                return false;
            obj.Comment = comment;
            bool data = await UtilityService.WriteAsync<AlertsDto>(alertsList, JsonFiles.ALERTS);
            AddClearedAlertIntoEvent(obj);
            return data;
        }

        public static void AddClearedAlertIntoEvent(AlertsDto Alerts)
        {
            var Eventjson = (UtilityService.Read<List<EventsDto>>(JsonFiles.EVENTS) ?? new List<EventsDto>()).AsQueryable();
            List<EventsDto> EventLists = Eventjson.ToList();
            int eventId = EventLists.Max(u => u.EventId);
            EventLists.Add(new EventsDto()
            {
                EventId = eventId+1,
                WellId = Alerts.WellId,
                WellName = Alerts.WellName,
                EventType = "Alerts",
                EventDescription = Alerts.Desc + " - " + Alerts.Comment,
                CreationDateTime = DateTime.Now,
                UpdatedBy = Alerts.UpdateBy
            });
            var filePath = JsonFiles.EVENTS;
            UtilityService.Write<EventsDto>(EventLists, filePath);


        }



        #endregion

        #region private methods

        /// <summary>
        /// GetWellsByWellNames Filter wells by Well Names
        /// </summary>
        /// <param name="alertListFilter">Filter object which contains pagination , sorting , searching , well name etc details to filter</param>
        /// <param name="alertsList">list to filter</param>
        /// <returns></returns>
        public static List<AlertsDto> GetWellsByWellNames(AlertListFilterDto alertListFilter, List<AlertsDto> alertsList)
        {

            var searchwells = alertsList.AsQueryable();
            if ((alertListFilter.WellNames != null))
            {
                String? Wellnames = alertListFilter.WellNames.FirstOrDefault();
                if (Wellnames != "" && Wellnames != null)
                {
                    var StatusSpecification = new AlertByWellNameSpecification(alertListFilter);
                    searchwells = searchwells.Where(StatusSpecification.ToExpression());
                    alertsList = searchwells.ToList();
                }
            }
            return alertsList;
        }

        /// <summary>
        /// GetWellbyCategorys Filter wells by category
        /// </summary>
        /// <param name="alertListFilter">Filter object which contains pagination , sorting , searching , well name etc details to filter</param>
        /// <param name="alertsList">list to filter</param>
        /// <returns></returns>
        public static List<AlertsDto> GetWellbyCategorys(AlertListFilterDto alertListFilter, List<AlertsDto> alertsList)
        {
            var searchwells = alertsList.AsQueryable();

            if (alertListFilter != null && alertListFilter.Category != null)
            {
                String? Categorys = alertListFilter.Category.FirstOrDefault();
                if (Categorys != "" && Categorys != null)
                {
                    var StatusSpecification = new AlertByCategorySpecification(alertListFilter);
                    searchwells = searchwells.Where(StatusSpecification.ToExpression());
                    alertsList = searchwells.ToList();
                }
            }
            return alertsList;
        }

        /// <summary>
        /// GetAllAlertsAndAlert 
        /// </summary>
        /// <param name="alertId">by alertId</param>
        /// <param name="alertsList">get alert list object</param>
        /// <param name="obj">get alert object</param>
        private static void GetAllAlertsAndAlert(int? alertId, out List<AlertsDto> alertsList, out AlertsDto? obj)
        {
            var alertsListJson = (UtilityService.Read<List<AlertsDto>>(JsonFiles.ALERTS) ?? new List<AlertsDto>()).AsQueryable();
            alertsList = alertsListJson.ToList();
            var alertSnoozeBySpecification = new AlertByIdSpecification(alertId);
            obj = alertsListJson.FirstOrDefault(alertSnoozeBySpecification.ToExpression());
        }




        /// <summary>
        /// DateRange filter list from spectified date range
        /// </summary>
        /// <param name="FromDate">from date range</param>
        /// <param name="ToDate">to date range</param>
        /// <param name="alertsList">list to filter</param>
        /// <returns>list of alerts filtered</returns>
        private static List<AlertsDto> DateRange(string FromDate, string ToDate, List<AlertsDto> alertsList)
        {
            var alerts = alertsList.AsQueryable();
            var dateRangeSpecification = new AlertsByDateRangeSpecification(FromDate, ToDate);
            alerts = alerts.Where(dateRangeSpecification.ToExpression());
            alertsList = alerts.ToList();
            return alertsList;
        }

        /// <summary>
        /// SearchText get list after filtering by search text
        /// </summary>
        /// <param name="SearchText">text to filter</param>
        /// <param name="alertsList">list to filter</param>
        /// <returns>list of alerts filtered</returns>
        private static List<AlertsDto> SearchText(string SearchText, List<AlertsDto> alertsList)
        {
            var alerts = alertsList.AsQueryable();
            var searchTextSpecification = new AlertsBySearchTextSpecification(SearchText.ToLower());
            alerts = alerts.Where(searchTextSpecification.ToExpression());
            alertsList = alerts.ToList();
            return alertsList;
        }

        /// <summary>
        /// Get AlertLevel details
        /// </summary>
        /// <param name="alertsList">list to filter</param>
        /// <returns>common alert details</returns>
        private static AlertsLevelDto AlertLevel(List<AlertsDto> alertsList)
        {
            var alerts = alertsList.AsQueryable();
            AlertsLevelDto alertsLevelDto = new()
            {
                TotalCount = alerts.Count()
            };

            var HighSpecification = new AlertsByLevelSpecification(CommonConstants.High);
            alertsLevelDto.TotalHigh = alerts.Where(HighSpecification.ToExpression()).Count();

            var MediumSpecification = new AlertsByLevelSpecification(CommonConstants.Medium);
            alertsLevelDto.TotalMedium = alerts.Where(MediumSpecification.ToExpression()).Count();

            var LowSpecification = new AlertsByLevelSpecification(CommonConstants.Low);
            alertsLevelDto.TotalLow = alerts.Where(LowSpecification.ToExpression()).Count();

            var ClearedSpecification = new AlertsByLevelSpecification(CommonConstants.Cleared);
            alertsLevelDto.TotalCleared = alerts.Where(ClearedSpecification.ToExpression()).Count();
            return alertsLevelDto;
        }

        /// <summary>
        /// Alertcountlist get count by priority
        /// </summary>
        /// <param name="alertsList">list to get count details</param>
        /// <returns>List<Alertcount> object</returns>
        private static List<Alertcount> Alertcountlist(List<AlertsDto> alertsList)
        {
            bool SnoozFlag = true;

            List<Alertcount> AlertNumber = new();
            var list = alertsList.GroupBy(x => x.WellName).Select(e => new {
                WellName = e.Key,
                Alertcount = e.Count(),
                SnoozAlertCount = e.Count(g => g.SnoozeFlag == SnoozFlag),
                High = e.Count(g => g.AlertLevel == CommonConstants.High),
                low = e.Count(g => g.AlertLevel == CommonConstants.Low),
                medium = e.Count(g => g.AlertLevel == CommonConstants.Medium),
                Cleared = e.Count(g => g.AlertLevel == CommonConstants.Cleared),
            }).ToList();


            foreach (var alert in list)
            {
                Alertcount alertcounts = new()
                {
                    Wellname = alert.WellName,
                    AlertCount = alert.Alertcount,
                    SnoozAlertCount = alert.SnoozAlertCount,
                    High = alert.High,
                    Low = alert.low,
                    Medium = alert.medium
                };
                AlertNumber.Add(alertcounts);
            }


            return AlertNumber;
        }


        /// <summary>
        /// AlertCategorieslist get category list with count of category
        /// </summary>
        /// <param name="alertsList">list to get count details</param>
        /// <returns>List<AlertCategory> obj</returns>
        private static List<AlertCategory> AlertCategorieslist(List<AlertsDto> alertsList)
        {

            List<AlertCategory> alertCategories = new();
            alertsList = alertsList.ToList();
            var data = alertsList.GroupBy(fu => fu.Category)

            .Select(g => new { Label = g.Key, Value = g.Count() * 100 / alertsList.Count })
            .ToList();


            foreach (var alert in data)
            {
                AlertCategory alertCategory = new()
                {
                    Value = alert.Value,
                    Name = alert.Label
                };
                alertCategories.Add(alertCategory);
            }



            return alertCategories;
        }


        /// <summary>
        /// SearchStatus alert serach by text
        /// </summary>
        /// <param name="SearchStatus">text to search</param>
        /// <param name="alertsList">list to filter</param>
        /// <returns> List<AlertsDto> object</returns>
        private static List<AlertsDto> SearchStatus(string SearchStatus, List<AlertsDto> alertsList)
        {
            var alerts = alertsList.AsQueryable();
            var alertsByLevelSpecification = new AlertsByLevelSpecification(SearchStatus.ToLower());
            alerts = alerts.Where(alertsByLevelSpecification.ToExpression());
            alertsList = alerts.ToList();
            return alertsList;
        }

        /// <summary>
        /// Sorting operation on list
        /// </summary>
        /// <param name="SortColumn">by sort column</param>
        /// <param name="SortDirection">by sort direction</param>
        /// <param name="alertsList">list to sort</param>
        /// <returns>sorted alert list </returns>
        private static List<AlertsDto> Sort(string SortColumn, string SortDirection, List<AlertsDto> alertsList)
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
            alertsList = alerts.ToList();
            return alertsList;
        }

        /// <summary>
        /// SnoozeByFalse
        /// </summary>
        /// <param name="alertsList">list to filter</param>
        /// <returns>List<AlertsDto> object after snooze by false</returns>
        private static List<AlertsDto> SnoozeByFalse(List<AlertsDto> alertsList)
        {
            bool SnoozbyFalse = false;
            var alerts = alertsList.AsQueryable();
            foreach (var alert in alerts.Where(a => a.SnoozeFlag))
            {
                var snInterval = alert.SnoozeInterval;
                var snDateTime = alert.SnoozeDateTime;
                var s = Convert.ToDateTime(snDateTime).AddHours(snInterval);
                if (s < DateTime.Now)
                    alert.SnoozeFlag = false;
                else
                    alert.SnoozeFlag = true;
            }
            alerts = alerts.Where(a => a.SnoozeFlag == SnoozbyFalse);
            alertsList = alerts.ToList();
            return alertsList;
        }


        #endregion

    }
}

