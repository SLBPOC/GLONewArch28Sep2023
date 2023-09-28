using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Delfi.Glo.Entities.Db
{
    public class DbBaseEntity
    {
        [Column("id", TypeName = "uuid")]
        public Guid Id { get; set; }
        [Column("entitytype")]
        [AllowNull]
        public string EntityType { get; set; }
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        // add other base properties if any
    }
}