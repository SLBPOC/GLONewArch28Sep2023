using Delfi.Glo.Api.Controllers;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Delfi.Glo.Api.Test.Controllers
{
    public class CustomAlertControllerTest
    {  
        CustomAlertController? _controller;
        private readonly ILogger<CustomAlertController> _mockLogger;
        public CustomAlertControllerTest()
        {
            this._mockLogger = new LoggerFactory().CreateLogger<CustomAlertController>();
        }
        [Fact()]
        public void Get()
        {            
            var mockService = new Mock<ICustomAlertService<CustomAlertDetailsDto>>(); 
            CustomAlertFilter customAlertFilter = new()
            {
                PageNumber = 1,
                PageSize = 5,
                SortDirection = "WellName",
                SortColumn = "asc"
            };
            mockService.Setup(p => p.GetCustomAlerts(customAlertFilter));
            _controller = new CustomAlertController(_mockLogger, mockService.Object);
            var searchResult = _controller.Get(customAlertFilter);
            Assert.NotNull(searchResult);
        }
        [Fact()]
        public void Post()
        {
            var mockService = new Mock<ICustomAlertService<CustomAlertDetailsDto>>();
            var customAlertDto = new CustomAlertDto()
            {
                CustomAlertName = "Custom Alert 15",
                WellName = "Jones 2-16-022BH1",
                Operator = "=",
                Value = "Any numerical value",
                Category = "Current SPM",
                StartDate = "2023-09-06T17:54:34.699Z",
                EndDate = "2023-19-06T17:54:34.699Z",
                NotificationType = "Text",
                Priority = "High",
                IsActive = true,                
                ActualValue = 50             
                
            };
            mockService.Setup(p => p.CreateCustomAlert(customAlertDto)).ReturnsAsync(true);
            _controller = new CustomAlertController(_mockLogger, mockService.Object);
            var result = _controller.Post(customAlertDto);
            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact()]
        public void PostNegative()
        {
            var mockService = new Mock<ICustomAlertService<CustomAlertDetailsDto>>();
            var customAlertDto = new CustomAlertDto();
            customAlertDto = null;
#pragma warning disable CS8604 // Possible null reference argument.
            mockService.Setup(p => p.CreateCustomAlert(customAlertDto)).ReturnsAsync(true);
            _controller = new CustomAlertController(_mockLogger, mockService.Object);
            var result =  _controller.Post(customAlertDto);
#pragma warning restore CS8604 // Possible null reference argument.
            Assert.IsType<BadRequestResult>(result.Result);
        }
        [Fact()]
        public void Delete()
        {
            var mockService = new Mock<ICustomAlertService<CustomAlertDetailsDto>>();
            mockService.Setup(p => p.DeleteCustomAlert(55)).ReturnsAsync(true);
            _controller = new CustomAlertController(_mockLogger, mockService.Object);
            var result =  _controller.Delete(55);            
            Assert.True(result.Result.Value);
        }
        [Fact()]
        public void Put()
        {
            var mockService = new Mock<ICustomAlertService<CustomAlertDetailsDto>>();
            mockService.Setup(p => p.UpdateCustomAlert(8,false)).ReturnsAsync(true);
            _controller = new CustomAlertController(_mockLogger, mockService.Object);
            var result = _controller.Put(8, false);
            Assert.True(result.Result.Value);
        }
        [Fact()]
        public void UpdateAlert()
        {
            var mockService = new Mock<ICustomAlertService<CustomAlertDetailsDto>>();
            var customAlertDto = new CustomAlertDto()
            {
                Id=55,
                CustomAlertName = "Custom Alert 15",
                WellName = "Jones 2-16-022BH1",
                Operator = "=",
                Value = "Any numerical value",
                Category = "Current SPM",
                StartDate = "2023-09-06T17:54:34.699Z",
                EndDate = "2023-19-06T17:54:34.699Z",
                NotificationType = "Text",
                Priority = "High",
                IsActive = true,
                ActualValue = 50
            };
            mockService.Setup(p => p.UpdateAlert(customAlertDto)).ReturnsAsync(true);
            _controller = new CustomAlertController(_mockLogger, mockService.Object);
            var result = _controller.Update(customAlertDto);
            Assert.True(result.Result.Value);
        }
    }
}
