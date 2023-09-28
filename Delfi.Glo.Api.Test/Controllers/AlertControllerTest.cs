using Delfi.Glo.Api.Controllers;
using Delfi.Glo.Common.Services;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.DataAccess.Specifications;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Delfi.Glo.Api.Test.Controllers
{
    public class AlertControllerTest
    {
        [AllowNull]
        private readonly ILogger<AlertsController> _mockLogger;
        public AlertControllerTest()
        {
            this._mockLogger = new LoggerFactory().CreateLogger<AlertsController>();
        }


        [Fact()]
        public void AlertsController_GetWellName()
        {
            AlertListFilterDto alertListFilter = new()
            {
                PageSize = 10,
                PageNumber = 1,
                SearchText = "",
                SortColumn = "",
                SortDirection = "",
                SearchStatus = "",
                DateRange = new DateRange
                {
                    FromDate = "",
                    ToDate = ""
                },
                WellNames = new string[]
      {
    "Jones 2-16-022BH2",
      },
                Category = new string[]
      {
       "",
      },
                Ids = Array.Empty<int>()
            };

            var AlertsService = new AlertsService();
            var alertcontroller = new AlertsController(AlertsService);
            IActionResult validatecontroller = alertcontroller.Get(alertListFilter).Result;
            OkObjectResult? okObject = validatecontroller as OkObjectResult;

            Assert.NotNull(okObject);
            var result = okObject.Value as AlertsDetailsDto;
            Assert.Equal(2, result?.Alerts?.Count);
            Assert.Equal(200, okObject.StatusCode);
        }

        [Fact()]
        public void AlertsController_GetWellById()
        {
            AlertListFilterDto alertListFilter = new()
            {
                PageSize = 10,
                PageNumber = 1,
                SearchText = "",
                SortColumn = "",
                SortDirection = "",
                SearchStatus = "",
                DateRange = new DateRange
                {
                    FromDate = "",
                    ToDate = ""
                },
                WellNames = new string[]
      {
    "",
      },
                Category = new string[]
      {
       "",
      },
                Ids = new int[]
      {
       12,
      }
            };

            var AlertsService = new AlertsService();
            var alertcontroller = new AlertsController(AlertsService);
            IActionResult validatecontroller = alertcontroller.Get(alertListFilter).Result;
            OkObjectResult? okObject = validatecontroller as OkObjectResult;

            Assert.NotNull(okObject);
            var result = okObject.Value as AlertsDetailsDto;
            Assert.Equal(2, result?.Alerts?.Count);
            Assert.Equal(200, okObject.StatusCode);
        }



        [Fact()]
        public void AlertsController_GetCategory()
        {
            AlertListFilterDto alertListFilter = new()
            {
                PageSize = 10,
                PageNumber = 1,
                SearchText = "",
                SortColumn = "",
                SortDirection = "",
                SearchStatus = "",
                DateRange = new DateRange
                {
                    FromDate = "",
                    ToDate = ""
                },
                WellNames = new string[]
      {
    "",
      },
                Category = new string[]
      {
       "Shutdowns",
      },
                Ids = Array.Empty<int>()
            };

            var AlertsService = new AlertsService();
            var alertcontroller = new AlertsController(AlertsService);
            IActionResult validatecontroller = alertcontroller.Get(alertListFilter).Result;
            OkObjectResult? okObject = validatecontroller as OkObjectResult;

            Assert.NotNull(okObject);
            var result = okObject.Value as AlertsDetailsDto;
            Assert.Equal(1, result?.Alerts?.Count);
            Assert.Equal(200, okObject.StatusCode);
        }

        [Fact()]
        public void AlertsController_GetNotNull()
        {
            AlertListFilterDto alertListFilter = new()
            {
                PageSize = 10,
                PageNumber = 1,
                SearchText = "",
                SortColumn = "",
                SortDirection = "",
                SearchStatus = ""
                
            };

            var AlertsService = new AlertsService();
            var alertcontroller = new AlertsController(AlertsService);
            IActionResult validatecontroller = alertcontroller.Get(alertListFilter).Result;
            OkObjectResult? okObject = validatecontroller as OkObjectResult;

            Assert.NotNull(okObject);
            var result = okObject.Value as AlertsDetailsDto;
            Assert.Equal(10, result?.Alerts?.Count);
            Assert.Equal(200, okObject.StatusCode);
        }


        [Fact()]
        public void AlertsController_GetSnoozbyWellsNotNull()
        {
            var AlertsService = new AlertsService();
            var alertcontroller = new AlertsController(AlertsService);
            string Wellname = "Apache 24 FED 11";
            IActionResult validatecontroller = alertcontroller.GetSnoozbyWells(Wellname).Result;
            OkObjectResult? okObject = validatecontroller as OkObjectResult;

            Assert.NotNull(validatecontroller);           
        }

       
        [Fact()]
        public void AlertsController_GetSnoozeByAlertsNotNull()
        {
            var AlertsService = new AlertsService();
            var controller = new AlertsController(AlertsService);
            int Alertid = 1;
            int Snoozby = 1;
            var searchResult = controller.GetSnoozeByAlert(Alertid, Snoozby);
            Assert.NotNull(searchResult);           
        }


        [Fact()]
        public  void AlertsController_ClearAlertAlertsNotNull()
        {
            var AlertsService = new AlertsService();
            var controller = new AlertsController(AlertsService);
            int Alertid = 1;
            String Comment = "test";
            var searchResult = controller.SetClearAlert(Alertid, Comment);
            Assert.NotNull(searchResult);
        }


        [Fact()]
        public void AlertsController_GetDefaultValuesNotNull()
        {
            var AlertsService = new AlertsService();
            var controller = new AlertsController(AlertsService);
            var searchResult = controller.GetDefaultValues();
            Assert.NotNull(searchResult);
        }

        [Fact()]
        public void AlertsController_UpdateSnoozeNotNull()
        {
            var AlertsService = new AlertsService();
            var controller = new AlertsController(AlertsService);
            var searchResult = controller.UpdateSnooze(1,1);
            Assert.NotNull(searchResult);
        }
    }
}
