
using Delfi.Glo.Entities.Db;
using Delfi.Glo.Repository;

namespace Delfi.Glo.PostgreSql.Dal
{
    public  class DbUnitWork
    {
        private readonly ApplicationContext _dbContext;

        public DbUnitWork(ApplicationContext applicationContext)
        {
            _dbContext = applicationContext;

        }
        private Repository<Crew> _crew;


        public IRepository<Crew> crews => _crew ??= new Repository<Crew>(_dbContext);


        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync(true);
        }
    }
}
