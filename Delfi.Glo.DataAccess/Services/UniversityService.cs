#pragma warning disable
using Delfi.Glo.Common.Services.Interfaces;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Delfi.Glo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Common.Constants;
using Microsoft.AspNetCore.Http;
using Delfi.Glo.DataAccess.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Delfi.Glo.DataAccess.Specifications;

namespace Delfi.Glo.DataAccess.Services
{
    public class UniversityService : IUniversityService<UniversitiesDto>
    {

        private readonly IHttpService<UniversitiesDto> httpService;

        public UniversityService(IHttpService<UniversitiesDto> httpService)
        {
            this.httpService = httpService;
        }

        public async Task<IEnumerable<UniversitiesDto>> GetUniversities(int page, int pageSize, string universityByName)
        {
            var universitiesInJson = UtilityService.Read<List<UniversitiesDto>>
                                                    (JsonFiles.UNIVERSITIES).AsQueryable();


            var spec = new UniversityByNameSpecification(universityByName);
            var universities = universitiesInJson.Where(spec.ToExpression()).Skip(page * pageSize)
                                                                            .Take(pageSize)
                                                                            .OrderByDescending(un => un.Name)
                                                                            .ToList();
            return universities;
        }
    }
}
#pragma warning restore