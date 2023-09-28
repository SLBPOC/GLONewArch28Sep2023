using Ardalis.GuardClauses;
using Delfi.Glo.Api.Exceptions;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Delfi.Glo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService<UniversitiesDto> universityService;

        public UniversityController(IUniversityService<UniversitiesDto> universityService)
        {
            this.universityService = universityService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get(int pageIndex, int pageSize, string universityName)
        {
            Guard.Against.InvalidPageIndex(pageIndex);
            Guard.Against.InvalidPageSize(pageSize);
            var result = await universityService.GetUniversities(pageIndex, pageSize, universityName);

            if (result != null && result?.Count() > 0) return Ok(result);
            else return NotFound($"No university found with name {universityName}");
        }
    }
}
