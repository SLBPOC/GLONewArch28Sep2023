using System.Diagnostics.CodeAnalysis;
using Delfi.Glo.Api.Controllers;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.Extensions.Logging;
using Delfi.Glo.DataAccess.Services;
using Moq;
using Delfi.Glo.Common.Services.Interfaces;

namespace Delfi.Glo.Api.Test.Controllers
{
    public class WellControllerTest
    {
        [AllowNull]
        private readonly ILogger<WellController> _mockLogger;
        private readonly IHttpService<WellDetailsDto> _mockHttpService;
        private readonly IWellHierarchyService _wellHierarchyService;
        private readonly IWellService<WellDetailsDto> _wellService;
        public WellControllerTest()
        {
            this._wellService = new WellService(_mockHttpService);
            this._mockLogger = new LoggerFactory().CreateLogger<WellController>();
            this._wellHierarchyService = new WellHierarchyService(_wellService);
        }
        [Fact()]
        public void GetWellListByFilters()
        {
            var mockHttpService = new Mock<IHttpService<WellDetailsDto>>();
            var wellListService = new WellService(mockHttpService.Object);
            var wellListController = new WellController(_mockLogger, wellListService, _wellHierarchyService);

            WellListFilterDto wellListFilter = new()
            {
                SearchText = string.Empty,
                PageNumber = 1,
                PageSize = 5
            };
            var searchResult = wellListController.Get(wellListFilter);
            Assert.NotNull(searchResult);
        }
        [Fact()]
        public void GetDefaultValues()
        {
            var mockHttpService = new Mock<IHttpService<WellDetailsDto>>();
            var wellListService = new WellService(mockHttpService.Object);
            var wellListController = new WellController(_mockLogger, wellListService, _wellHierarchyService);
            var searchResult = wellListController.GetDefaultValues();
            Assert.NotNull(searchResult);
        }
        [Fact()]
        public void Get()
        {
            var mockHttpService = new Mock<IHttpService<WellDetailsDto>>();
            var wellListService = new WellService(mockHttpService.Object);
            var wellListController = new WellController(_mockLogger, wellListService, _wellHierarchyService);
            List<NodeType> nodeType = new()
            {
                NodeType.Field,
                NodeType.Battery,
                NodeType.Pad,
                NodeType.Wells
            };
            WellHierarchyRequest wellHierarchyRequest = new()
            {
                SearchText = "Battery",
                SearchLevels = nodeType,
            };
            var searchResult = wellListController.Get(wellHierarchyRequest);
            Assert.NotNull(searchResult);
        }
    }
}
