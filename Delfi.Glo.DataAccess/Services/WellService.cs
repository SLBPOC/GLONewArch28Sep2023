using Delfi.Glo.Common.Services;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Delfi.Glo.Common.Constants;
using Range = Delfi.Glo.Entities.Dto.Range;
using Delfi.Glo.DataAccess.Specifications;
using Delfi.Glo.Common.Services.Interfaces;

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
            if (wellsInJson != null)
            {
                var wells = wellsInJson.Select(a => new WellIdName { WellId = a.WellId, WellName = a.WellName }).DistinctBy(a => a.WellName).ToList();
                defaultWellFilterValues.WellNames = wells;
                var commStatus = wellsInJson.Select(a => new IsChecked { Value = a.CommStatus, Checked = false }).DistinctBy(a => a.Value).ToList();
                defaultWellFilterValues.CommStatus = commStatus;

                var controllerStatus = wellsInJson.Select(a => new IsChecked { Value = a.ControllerStatus, Checked = false }).DistinctBy(a => a.Value).ToList();
                defaultWellFilterValues.ControllerStatus = controllerStatus;

                var pumpingType = wellsInJson.Select(a => new IsChecked { Value = a.WellStatus, Checked = false }).DistinctBy(a => a.Value).ToList();
                defaultWellFilterValues.PumpingTypes = pumpingType;
            }
            var spm = new Range { Start = 0, End = 20, Min = 0, Max = 20 };
            defaultWellFilterValues.SpmSlider = spm;

            var pumpFillageSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.PumpFillageSlider = pumpFillageSlider;

            var inferredProductionSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.InferredProductionSlider = inferredProductionSlider;

            var effectiveRuntimeSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.EffectiveRuntimeSlider = effectiveRuntimeSlider;

            var cyclesTodaySlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.CyclesTodaySlider = cyclesTodaySlider;

            var structuralLoadSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.StructuralLoadSlider = structuralLoadSlider;

            var minMaxLoadSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.MinMaxLoadSlider = minMaxLoadSlider;

            var gearboxLoadSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.GearboxLoadSlider = gearboxLoadSlider;

            var rodStressSlider = new Range { Start = 0, End = 100, Min = 0, Max = 100 };
            defaultWellFilterValues.RodStressSlider = rodStressSlider;
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
                wellsInJson = GetWellsBySearchText(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByCommStatus(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByControllerStatus(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByPumpingType(wellListFilter, wellsInJson);
                wellsInJson = GetWellsBySPM(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByPumpFillage(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByInferredProduction(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByEffectiveRuntime(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByCyclesToday(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByStructuralLoad(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByMinMaxLoad(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByGearboxLoad(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByRodStress(wellListFilter, wellsInJson);
                wellsInJson = GetWellsByWellNames(wellListFilter, wellsInJson);
                wellDetailsDto.PumpingDetails = GetPumpingDetails(wellsInJson);
                if (wellListFilter.SearchStatus != null && wellListFilter.SearchStatus != "")
                {
                    wellsInJson = GetWellsBySearchStatus(wellListFilter, wellsInJson);
                    wellDetailsDto.PumpingDetails = GetPumpingDetails(wellsInJson);
                    wellDetailsDto.PumpingDetails.OptimalPumping = wellDetailsDto.PumpingDetails.OptimalPumping == 0 ? wellsJson.Where(c => c.WellStatus == CommonConstants.OptimumPumping).Count() : wellDetailsDto.PumpingDetails.OptimalPumping;
                    wellDetailsDto.PumpingDetails.OverPumping = wellDetailsDto.PumpingDetails.OverPumping == 0 ? wellsJson.Where(c => c.WellStatus == CommonConstants.OverPumping).Count() : wellDetailsDto.PumpingDetails.OverPumping;
                    wellDetailsDto.PumpingDetails.UnderPumping = wellDetailsDto.PumpingDetails.UnderPumping == 0 ? wellsJson.Where(c => c.WellStatus == CommonConstants.UnderPumping).Count() : wellDetailsDto.PumpingDetails.UnderPumping;
                }
                wellsInJson = GetWellsByWellHierarchyIds(wellListFilter, wellsInJson);
                var finalResults = UtilityService.SortAndPagination(wellListFilter.SortColumn == null ? "" : wellListFilter.SortColumn
                                                                  , wellListFilter.SortDirection == null ? "" : wellListFilter.SortDirection
                                                                  , wellListFilter.PageNumber, wellListFilter.PageSize, wellsInJson);
                wellDetailsDto.WellDtos = (List<WellDto>?)finalResults;

            }
            return wellDetailsDto;
        }

        #endregion


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
        private static IQueryable<WellDto> GetWellsBySearchText(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.PumpingType != null && wellListFilter.PumpingType.Length > 0)
            {
                var StatusSpecification = new WellsByPumpingTypeSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByEffectiveRuntime Get well filters after effective runtime
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByEffectiveRuntime(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.EffectiveRuntime != null && wellListFilter.EffectiveRuntime.Start >= 0 && wellListFilter.EffectiveRuntime.End > 0)
            {
                var StatusSpecification = new WellsByEffectiveRuntimeSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByCyclesToday Get well filters after cycles today
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByCyclesToday(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.CyclesToday != null && wellListFilter.CyclesToday.Start >= 0 && wellListFilter.CyclesToday.End > 0)
            {
                var StatusSpecification = new WellsByCyclesTodaySpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByStructuralLoad Get well filters after structuaral load
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByStructuralLoad(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.StructuralLoad != null && wellListFilter.StructuralLoad.Start >= 0 && wellListFilter.StructuralLoad.End > 0)
            {
                var StatusSpecification = new WellsByStructuralLoadSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByMinMaxLoad Get well filters after minmax load
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByMinMaxLoad(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.MinMaxLoad != null && wellListFilter.MinMaxLoad.Start >= 0 && wellListFilter.MinMaxLoad.End > 0)
            {
                var StatusSpecification = new WellsByMinMaxLoadSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByGearboxLoad Get well filters after gearbox load
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByGearboxLoad(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.GearboxLoad != null && wellListFilter.GearboxLoad.Start >= 0 && wellListFilter.GearboxLoad.End > 0)
            {
                var StatusSpecification = new WellsByGearboxLoadSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByRodStress Get well filters after rod stress
        /// </summary>
        /// <param name="wellListFilter"></param>
        /// <param name="wellsInJson"></param>
        /// <returns></returns>
        private static IQueryable<WellDto> GetWellsByRodStress(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.RodStress != null && wellListFilter.RodStress.Start >= 0 && wellListFilter.RodStress.End > 0)
            {
                var StatusSpecification = new WellsByRodStressSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByCommStatus Get well filters after communication status
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByCommStatus(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.CommStatus != null && wellListFilter.CommStatus.Length > 0)
            {
                var StatusSpecification = new WellsByCommsStatusSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByControllerStatus   Get well filters after controller status
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByControllerStatus(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.ControllerStatus != null && wellListFilter.ControllerStatus.Length > 0)
            {
                var StatusSpecification = new WellsByControllerStatusSpecification(wellListFilter);
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
        /// GetWellsBySPM   Get well filters after SPM range
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsBySPM(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.SPM != null && wellListFilter.SPM.Start >= 0 && wellListFilter.SPM.End > 0)
            {
                var StatusSpecification = new WellsBySpmSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByInferredProduction  Get well filters after Inferred Production
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByInferredProduction(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.InferredProduction != null && wellListFilter.InferredProduction.Start >= 0 && wellListFilter.InferredProduction.End > 0)
            {
                var StatusSpecification = new WellsByInferredProductionSpecification(wellListFilter);
                searchwells = searchwells.Where(StatusSpecification.ToExpression());
                wellsInJson = searchwells;
            }
            return wellsInJson;
        }
        /// <summary>
        /// GetWellsByPumpFillage  Get well filters after pump fillage
        /// </summary>
        /// <param name="wellListFilter">Filter object which contains pagination , sorting , searching , well names and other filter details</param>
        /// <param name="wellsInJson">List to filter</param>
        /// <returns>WellDto object contains filtered wells</returns>
        private static IQueryable<WellDto> GetWellsByPumpFillage(WellListFilterDto wellListFilter, IQueryable<WellDto> wellsInJson)
        {
            var searchwells = wellsInJson;
            if (wellListFilter.PumpFillage != null && wellListFilter.PumpFillage.Start >= 0 && wellListFilter.PumpFillage.End > 0)
            {
                var StatusSpecification = new WellsByPumpFillageSpecification(wellListFilter);
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
