using Delfi.Glo.Entities.Db;
using Microsoft.EntityFrameworkCore;

namespace Delfi.Glo.PostgreSql.Dal
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Crew> Crew { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }        

    }
}
