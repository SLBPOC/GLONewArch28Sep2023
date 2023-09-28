using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Delfi.Glo.Entities.Db
{
    [Table("Well")]
    public class Well
    {
        [Column("id", TypeName = "int")]
        public int Id { get; set; }
        [AllowNull]
        public string WellId { get; set; }
        [AllowNull]
        public string WellName { get; set; }
        [AllowNull]
        public string DateAndTime { get; set; }
        [AllowNull]
        public string CommStatus { get; set; }
        [AllowNull]
        public string ControllerStatus { get; set; }
        public decimal SPM { get; set; }
        public decimal PumpFillage { get; set; }
        public decimal InferredProduction { get; set; }
        public int EffectiveRunTime { get; set; }
        public decimal CyclesToday { get; set; }
        public decimal StructuralLoad { get; set; }
        public decimal MinMaxLoad { get; set; }
        public decimal GearboxLoad { get; set; }
        public decimal RodStress { get; set; }
        public int NoOfAlerts { get; set; }
        [AllowNull]
        public string WellStatus { get; set; }
    }
}
