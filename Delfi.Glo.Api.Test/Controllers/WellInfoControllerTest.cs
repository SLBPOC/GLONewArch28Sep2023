using Delfi.Glo.Api.Controllers;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.PostgreSql.Dal.Services;
using Delfi.Glo.Repository;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delfi.Glo.DataAccess.Services;

namespace Delfi.Glo.Api.Test.Controllers
{
    public class WellInfoControllerTest
    {
        [AllowNull]
        private readonly ILogger<WellInfoController> _mockLogger;
        private readonly IWellInfoService<WellInfoDto> _wellInfoService;
        public WellInfoControllerTest()
        {
            this._mockLogger = new LoggerFactory().CreateLogger<WellInfoController>();
            this._wellInfoService = new WellInfoService();
        }
        [Fact()]
        public void Get()
        {
            var controller = new WellInfoController(_mockLogger, _wellInfoService);
            string wellId = "W001";
            var searchResult = controller.Get(wellId);
            Assert.NotNull(searchResult);
        }
    }
}
