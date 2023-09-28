using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delfi.Glo.Entities.Db
{
    [Table("Crew")]
    public class Crew : DbBaseEntity
    {
        public string? CrewName { get; set; }
    }
}
