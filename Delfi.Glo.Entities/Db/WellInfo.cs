using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Delfi.Glo.Entities.Db
{
    public class LastWellTestData
    {
        public int TestGasData { get; set; }
        public decimal TestOil { get; set; }
        public decimal TestWater { get; set; }
        public decimal TestGross { get; set; }
        [AllowNull]
        public string LastReceivedDtTime { get; set; }
    }

    public class PumpInfo
    {
        [AllowNull]
        public string PumpId { get; set; }
        [AllowNull]
        public string RodDetails { get; set; }
    }

    [Table("Well")]
    public class WellInfo
    {
        [Column("id", TypeName = "int")]
        public int Id { get; set; }
        [AllowNull]
        public string WellId { get; set; }
        [AllowNull]
        public string WellName { get; set; }
        [AllowNull]
        public string DateTime { get; set; }
        [AllowNull]
        public string CommsStatus { get; set; }
        [AllowNull]
        public string ControllerStatus { get; set; }
        public decimal Spm { get; set; }
        public decimal PumpFillage { get; set; }
        public decimal InferredProduction { get; set; }
        public int EffectiveRuntimePercentage { get; set; }
        public decimal CyclesToday { get; set; }
        public decimal StructuralLoadPercentage { get; set; }
        public decimal MinMaxloadPercentage { get; set; }
        public decimal GearBoxLoadPercentage { get; set; }
        public decimal RodStressPercentage { get; set; }
        public int NoOfAlerts { get; set; }
        [AllowNull]
        public string WellStatus { get; set; }
        [AllowNull]
        public string AvgSpm { get; set; }
        [AllowNull]
        public string WellState { get; set; }
        [AllowNull]
        public string PumpCardDiagnistics { get; set; }
        [AllowNull]
        public string PumpDisplacement { get; set; }
        public bool IsWellOffline { get; set; }
        [AllowNull]
        public string SurfaceUnitInformation { get; set; }
        [AllowNull]
        public string LastScan { get; set; }
        [AllowNull]
        public string GatewayId { get; set; }
        public int StrokeLengthInInches { get; set; }
        [AllowNull]
        public string TubingHeadPressure { get; set; }
        [AllowNull]
        public string CasingHeadPressure { get; set; }
        [AllowNull]
        public string CalculatedPumpIntakePressure { get; set; }
        public int CurrentFluidLoad { get; set; }
        public int DownholeCardAreaInSqInch { get; set; }
        [AllowNull]
        public PumpInfo PumpInfo { get; set; }
        public int PumpDepthInFt { get; set; }
        [AllowNull]
        public string PumpType { get; set; }
        [AllowNull]
        public LastWellTestData LastWellTestData { get; set; }
        [AllowNull]
        public string Load { get; set; }
    }
}
