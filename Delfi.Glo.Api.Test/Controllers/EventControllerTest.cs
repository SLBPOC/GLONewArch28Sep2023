using Castle.Components.DictionaryAdapter.Xml;
using Delfi.Glo.Api.Controllers;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NLog.Filters;

namespace Delfi.Glo.Api.Test.Controllers
{
    public class EventControllerTest
    {

        [Fact]
        public void Get_EventsFound_ReturnsOkResult()
        {
            var filter = new EventListFilterDto
            {
                PageSize = 5,
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
                },
                EventTypes = new string[]
                {
                },
                Ids = new int[]
                {
                }
            };

           
            var EventService = new EventService();
            var eventcontroller = new EventController(EventService);
            IActionResult validatecontroller = eventcontroller.Get(filter).Result;
            OkObjectResult? okObject = validatecontroller as OkObjectResult;

            Assert.NotNull(okObject);
            var result = okObject.Value as EventDetailsDto;
            Assert.Equal(5, result?.Events?.Count);
            Assert.Equal(200, okObject.StatusCode);
        }


        [Fact]
        public void Get_NoEventFound_ReturnsNotFound()
        {
            var filter = new EventListFilterDto
            {
                PageSize = 10,
                PageNumber = 1,
                SearchText = "test",
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
                EventTypes = new string[]
                {
                    "test",
                },
                Ids = new int[]
                {
                    0
                }
            };

            var EventService = new EventService();
            var eventcontroller = new EventController(EventService);
            IActionResult validatecontroller = eventcontroller.Get(filter).Result;
            NotFoundObjectResult? notFoundObject = validatecontroller as NotFoundObjectResult;
            Assert.NotNull(notFoundObject);
            Assert.Equal("No well found.", notFoundObject.Value);
            Assert.Equal(404, notFoundObject.StatusCode);
        }



        [Fact]
        public async Task Get_DefaultValuesFound_ReturnsOkResult()
        {
            var EventService = new EventService();
            var controller = new EventController(EventService);
            var okObject = await controller.Get();
            var okResult = Assert.IsType<OkObjectResult>(okObject);
            var result = okResult.Value as EventsDropDownDto;
            Assert.NotNull(okObject);
            Assert.NotNull(result?.EventTypes);
            Assert.NotNull(result.WellNames);
            Assert.Equal(200, okResult.StatusCode);
        }
        

        [Fact()]
        public async Task GetDefaultValues()
        {
            var EventService = new EventService();
            var controller = new EventController(EventService);
            var searchResult =await controller.Get();
            Assert.NotNull(searchResult);
        }
    }
}
