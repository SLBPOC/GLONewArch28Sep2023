using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Delfi.Glo.Common.Constants;
using Range = Delfi.Glo.Entities.Dto.Range;
using Delfi.Glo.DataAccess.Specifications;
using Delfi.Glo.Common.Services.Interfaces;
using System.Linq;

namespace Delfi.Glo.DataAccess.Services
{
    public class WellService : IWellService<WellDetailsDto>
    {
        #region public method
        private readonly IHttpService<WellDetailsDto> httpService;
        public WellService(IHttpService<WellDetailsDto> httpService)
        {
            this.httpService = httpService;
        }
        /// <summary>
        /// Get All wells        
        /// </summary>
        /// <returns>Get all wells</returns>
        public async Task<IQueryable<WellDto>?> GetWells()
        {
            return (await UtilityService.ReadAsync<List<WellDto>>(JsonFiles.WELLS))?.AsQueryable();
        }

        /// <summary>
        /// Get Default filter values
        /// </summary>
        /// <returns>Returns DefaultWellFilterValues object contains default filter values </returns>
        public async Task<DefaultWellFilterValues> GetWellFilterDefaultValues()
        {
            var wellsInJson = (await UtilityService.ReadAsync<List<WellDto>>(JsonFiles.WELLS))?.AsQueryable();
            DefaultWellFilterValues defaultWellFilterValues = new();
            //if (wellsInJson != null)
            //{
            //    var wells = wellsInJson.Select(a => new WellIdName { WellId = a.WellId, WellName = a.WellName }).DistinctBy(a => a.WellName).ToList();
            //    defaultWellFilterValues.WellNames = wells;
            //    var commStatus = wellsInJson.Select(a => new IsChecked { Value = a.CommStatus, Checked = false }).DistinctBy(a => a.Value).ToList();
            //    defaultWellFilterValues.CommStatus = commStatus;

            //    var controllerStatus = wellsInJson.Select(a => new IsChecked { Value = a.ControllerStatus, Checked = false }).DistinctBy(a => a.Value).ToList();
            //    defaultWellFilterValues.ControllerStatus = controllerStatus;

            //    var pumpingType = wellsInJson.Select(a => new IsChecked { Value = a.WellStatus, Checked = false }).DistinctBy(a => a.Value).ToList();
            //    defaultWellFilterValues.PumpingTypes = pumpingType;
            //}
            //var spm = new Range { Start = 0, End = 20, Min = 0, Max = 20 };
            //defaultWellFilterValues.SpmSlider = spm;

            //var pumpFillageSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.PumpFillageSlider = pumpFillageSlider;

            //var inferredProductionSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.InferredProductionSlider = inferredProductionSlider;

            //var effectiveRuntimeSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.EffectiveRuntimeSlider = effectiveRuntimeSlider;

            //var cyclesTodaySlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.CyclesTodaySlider = cyclesTodaySlider;

            //var structuralLoadSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.StructuralLoadSlider = structuralLoadSlider;

            //var minMaxLoadSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.MinMaxLoadSlider = minMaxLoadSlider;

            //var gearboxLoadSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.GearboxLoadSlider = gearboxLoadSlider;

            //var rodStressSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            //defaultWellFilterValues.RodStressSlider = rodStressSlider;
            return defaultWellFilterValues;
        }

        /// <summary>
        /// Get well List After filter
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <returns>return WellDetailsDto object contains all well details</returns>
        public async Task<WellDetailsDto> GetWellListByFilters(WellListFilterDto wellListFilter)
       {
            var wellsInJson = await GetWells();
            var wellsJson = await GetWells();
            WellDetailsDto wellDetailsDto = new();
            WellDetailsDto wellDetailsDtoNull = new();
            if (wellsJson == null) { return wellDetailsDtoNull; }
            if (wellsInJson != null)
            {
              //  wellsInJson = GetWellsBySearchText(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByLastCycleStatus(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByLastCycleStatus(wellListFilter, wellsInJson);

                wellsInJson = GetWellsByModeOfOperation(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByModeOfApproval(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByDeviceUpTime(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByCompressorUpTime(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByProductionUpTime(wellListFilter, wellsInJson);

                wellsInJson = GetWellsByWellNames(wellListFilter, wellsInJson);
                wellDetailsDto.currentcycleStatusCount = CycleStatusCountList(wellsInJson);
                wellDetailsDto.modeOfOperationCount=ModeOfOperationList(wellsInJson);
                wellsInJson = GetWellsByWellHierarchyIds(wellListFilter, wellsInJson);
                var finalResults = UtilityService.SortAndPagination(wellListFilter.SortColumn == null ? "" : wellListFilter.SortColumn
                                                                  , wellListFilter.SortDirection == null ? "" : wellListFilter.SortDirection
                                                                  , wellListFilter.PageNumber, wellListFilter.PageSize, wellsInJson);
                wellDetailsDto.WellDtos = (List<WellDto>?)finalResults;

            }
            return wellDetailsDto;
        }

        #endregion

        private static CycleStatusCount CycleStatusCountList(IQueryable<WellDto> wellsInJson)
        {
            CycleStatusCount cycleStatusCount = new();

            var StablilizationSpecification = new WellsCurrentCycleStatusSpecifincation(CommonConstants.Stabilization);
            cycleStatusCount.Stabilization = wellsInJson.Where(StablilizationSpecification.ToExpression()).Count();

            var RateEstimationSpecification = new WellsCurrentCycleStatusSpecifincation(CommonConstants.RateEstimation);
            cycleStatusCount.RateEstimation = wellsInJson.Where(RateEstimationSpecification.ToExpression()).Count();

            var WaitingForApprovalSpecification = new WellsCurrentCycleStatusSpecifincation(CommonConstants.WaitingForApproval);
            cycleStatusCount.WaitingForApproval = wellsInJson.Where(WaitingForApprovalSpecification.ToExpression()).Count();
            return cycleStatusCount;
        }

        private static ModeOfOperationCount ModeOfOperationList(IQueryable<WellDto> wellsInJson)
        {
            ModeOfOperationCount modeOfOperationCount = new();

            var ApprovedSpecification = new WellsModeOfOperationSpecifincation(CommonConstants.Approved);
            modeOfOperationCount.Approved = wellsInJson.Where(ApprovedSpecification.ToExpression()).Count();

            var WaitingForApprovalSpecification = new WellsModeOfOperationSpecifincation(CommonConstants.WaitingForApproval);
            modeOfOperationCount.WaitingForApproval = wellsInJson.Where(WaitingForApprovalSpecification.ToExpression()).Count();

            var DisCardSpecification = new WellsModeOfOperationSpecifincation(CommonConstants.Discard);
            modeOfOperationCount.Discard = wellsInJson.Where(DisCardSpecification.ToExpression()).Count();

            var OverriderSpecification = new WellsModeOfOperationSpecifincation(CommonConstants.Override);
            modeOfOperationCount.Override = wellsInJson.Where(OverriderSpecification.ToExpression()).Count();

            return modeOfOperationCount;
        }
        #region Private methods
        /// <summary>
        /// GetWellsByPumpingType Get well filters after pumping type
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByPumpingType(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.SearchText != null && wellListFilter.SearchText != "")
            {
                var searchTextspec = new WellsBySearchTextSpecification((wellListFilter.SearchText ?? "").ToLower());
                searchwells = searchwells.Where(searchTextspec.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsBySearchText Get well filters after search text
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        //private static IQueryable<WellDto> GetWellsBySearchText(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        //{
        //    var searchwells = wellsInJson;
        //    if (wellListFilter.PumpingType != null && wellListFilter.PumpingType.Length > 0)
        //    {
        //        var StatusSpecification = new WellsByPumpingTypeSpecification(wellListFilter);
        //        searchwells = searchwells.Where(StatusSpecification.ToExpression());
        //        wellsInJson = searchwells;
        //    }
        //    return wellsInJson;
        //}
        ///// <summary>
        ///// GetWellsByEffectiveRuntime Get well filters after effective runtime
        ///// </summary>
        ///// <param name="wellListFilter"></param>
        ///// <param name="wellsInJson"></param>
        ///// <returns></returns>
        //private static IQueryable<WellDto> GetWellsByEffectiveRuntime(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        //{
        //    var searchwells = wellsInJson;
        //    if (wellListFilter.EffectiveRuntime != null && wellListFilter.EffectiveRuntime.Start >= 0 && wellListFilter.EffectiveRuntime.End > 0)
        //    {
        //        var StatusSpecification = new WellsByEffectiveRuntimeSpecification(wellListFilter);
        //        searchwells = searchwells.Where(StatusSpecification.ToExpression());
        //        wellsInJson = searchwells;
        //    }
        //    return wellsInJson;
        //}
        ///// <summary>
        ///// GetWellsByCyclesToday Get well filters after cycles today
        ///// </summary>
        ///// <param name="wellListFilter"></param>
        ///// <param name="wellsInJson"></param>
        ///// <returns></returns>
        //private static IQueryable<WellDto> GetWellsByCyclesToday(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        //{
        //    var searchwells = wellsInJson;
        //    if (wellListFilter.CyclesToday != null && wellListFilter.CyclesToday.Start >= 0 && wellListFilter.CyclesToday.End > 0)
        //    {
        //        var StatusSpecification = new WellsByCyclesTodaySpecification(wellListFilter);
        //        searchwells = searchwells.Where(StatusSpecification.ToExpression());
        //        wellsInJson = searchwells;
        //    }
        //    return wellsInJson;
        //}
        ///// <summary>
        ///// GetWellsByStructuralLoad Get well filters after structuaral load
        ///// </summary>
        ///// <param name="wellListFilter"></param>
        ///// <param name="wellsInJson"></param>
        ///// <returns></returns>
        //private static IQueryable<WellDto> GetWellsByStructuralLoad(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        //{
        //    var searchwells = wellsInJson;
        //    if (wellListFilter.StructuralLoad != null && wellListFilter.StructuralLoad.Start >= 0 && wellListFilter.StructuralLoad.End > 0)
        //    {
        //        var StatusSpecification = new WellsByStructuralLoadSpecification(wellListFilter);
        //        searchwells = searchwells.Where(StatusSpecification.ToExpression());
        //        wellsInJson = searchwells;
        //    }
        //    return wellsInJson;
        //}
        ///// <summary>
        ///// GetWellsByMinMaxLoad Get well filters after minmax load
        ///// </summary>
        ///// <param name="wellListFilter"></param>
        ///// <param name="wellsInJson"></param>
        ///// <returns></returns>
        //private static IQueryable<WellDto> GetWellsByMinMaxLoad(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        //{
        //    var searchwells = wellsInJson;
        //    if (wellListFilter.MinMaxLoad != null && wellListFilter.MinMaxLoad.Start >= 0 && wellListFilter.MinMaxLoad.End > 0)
        //    {
        //        var StatusSpecification = new WellsByMinMaxLoadSpecification(wellListFilter);
        //        searchwells = searchwells.Where(StatusSpecification.ToExpression());
        //        wellsInJson = searchwells;
        //    }
        //    return wellsInJson;
        //}
        /// <summary>
        /// GetWellsByGearboxLoad Get well filters after gearbox load
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByProductionUpTime(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {

            var searchwells = wellsInJson;
            if (wellListFilter.ProductionUpTime != null && wellListFilter.ProductionUpTime.Length > 0)
            {
                var StatusSpecification = new WellsByProductionUpTimeSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByCompressorUpTime Get well filters after CompressorUpTime
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByCompressorUpTime(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
 
            var searchwells = wellsInJson;
            if (wellListFilter.CompressorUpTime != null && wellListFilter.CompressorUpTime.Length > 0)
            {
                var StatusSpecification = new WellsByCompressorUpTimeSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByModeOfApproval Get well filters after ModeOfApproval
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByModeOfApproval(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.ModeOfApproval != null && wellListFilter.ModeOfApproval.Length > 0)
            {
                var StatusSpecification = new WellsByModeOfApprovalSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByDeviceUpTime   Get well filters after DeviceUpTime
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByDeviceUpTime(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.DeviceUpTime != null && wellListFilter.DeviceUpTime.Length > 0)
            {
                var StatusSpecification = new WellsByDeviceUpTimeSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetPumpingDetails common legend details
        /// </summary>
        /// <param name="wellsInJson">wells to get common details from it</param>
        /// <returns>PumpingDetails object</returns>
        private static PumpingDetails GetPumpingDetails(IQueryable<WellDto> wellsInJson)
        {
            PumpingDetails pumpingDetails = new()
            {
                TotalCount = wellsInJson.Count()
            };

            var OverPumpingSpecification = new WellsByStatusSpecification(CommonConstants.OverPumping);
            pumpingDetails.OverPumping = wellsInJson.Where(OverPumpingSpecification.ToExpression()).Count();


            var OptimalPumpingSpecification = new WellsByStatusSpecification(CommonConstants.OptimumPumping);
            pumpingDetails.OptimalPumping = wellsInJson.Where(OptimalPumpingSpecification.ToExpression()).Count();

            var UnderPumpingSpecification = new WellsByStatusSpecification(CommonConstants.UnderPumping);
            pumpingDetails.UnderPumping = wellsInJson.Where(UnderPumpingSpecification.ToExpression()).Count();

            return pumpingDetails;
        }
        /// <summary>
        /// GetWellsByLastCycleStatus   Get well filters after LastCycleStatus
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByLastCycleStatus(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.LastCycleStatus != null && wellListFilter.LastCycleStatus.Length > 0)
            {
                var StatusSpecification = new WellsByLastCycleStatusSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByCurrentCycleStatus  Get well filters after CurrentCycleStatus
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByCurrentCycleStatus(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.CurrentCycleStatus != null && wellListFilter.CurrentCycleStatus.Length > 0)
            {
                var StatusSpecification = new WellsByCurrentCycleStatusSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByModeOfOperation  Get well filters after ModeOfOperation
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByModeOfOperation(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {


            var searchwells = wellsInJson;
            if (wellListFilter.ModeOfOperation != null && wellListFilter.ModeOfOperation.Length > 0)
            {
                var StatusSpecification = new WellsByModeOfOperationSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;

        }
        /// <summary>
        /// GetWellsByWellNames  Get well filters after well name
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByWellNames(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.WellNames != null && wellListFilter.WellNames.Any())
            {
                var StatusSpecification = new WellsByWellNamesSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsBySearchStatus  Get well filters after legends search status
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsBySearchStatus(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.SearchStatus != null && wellListFilter.SearchStatus != "")
            {
                var StatusSpecification = new WellsByStatusSpecification(wellListFilter.SearchStatus);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByWellHierarchyIds  Get well filters after well ids from well hiearachy
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByWellHierarchyIds(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.Ids != null && wellListFilter.Ids.Any())
            {
                var StatusSpecification = new WellsByWellHierarchyIdsSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        #endregion
    }
}
