#pragma warning disable
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Delfi.Glo.Entities.Db;
using Delfi.Glo.Entities.Enums;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.Repository;
using Delfi.Glo.Common.Services.Interfaces;
using Delfi.Glo.PostgreSql.Dal;

namespace Delfi.Glo.DataAccess.Services
{
    public class CrewService : ICrudService<CrewDto>
    {

        private readonly DbUnitWork _dbUnit;

        private ApplicationContext _dbContext;

        public CrewService(DbUnitWork dbUnit, ApplicationContext dbContext)
        {
            _dbUnit = dbUnit;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CrewDto>> GetAllAsync()
        {



            var crews = _dbUnit.crews.GetAll().ToList();
            var crewsDto = new List<CrewDto>();
            foreach (var crew in crews)
            {
                var crewDto = new CrewDto();
                crewDto.CrewName = crew.CrewName;
                crewDto.Id = crew.Id;
                crewsDto.Add(crewDto);
            }
            return crewsDto;
        }

        public async Task<bool> ExistsAsync(Guid id) => throw new NotImplementedException();

        public async Task<CrewDto> CreateAsync(CrewDto crew)
        {
            Crew _crew = new Crew();
            _crew.CrewName = crew.CrewName;

            _dbUnit.crews.Create(_crew);
            await _dbUnit.SaveChangesAsync();
            return crew;
        }

        public async Task<CrewDto> UpdateAsync(Guid id, CrewDto crew)
        {
            Crew _crew = new Crew();
            _crew.Id = crew.Id;
            _crew.CrewName = crew.CrewName;

            _dbUnit.crews.Update(_crew);
            await _dbUnit.SaveChangesAsync();
            return crew;
        }



        public async Task<bool> DeleteAsync(Guid id)
        {
            Crew obj = _dbUnit.crews.FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return false;
            }
            _dbUnit.crews.Delete(obj);
            _dbUnit.SaveChangesAsync();
            return true;
        }

        public async Task<CrewDto> GetAsync(int id)
        {
            if (id == null)
            {
                return null;
            }
            var a = await _dbContext.Crew.FindAsync(id);
            CrewDto crew = new CrewDto();
            crew.Id = a.Id;
            crew.CrewName = a.CrewName;
            return crew;
        }



        public Task<CrewDto> UpdateAsync(int id, CrewDto item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
#pragma warning restore