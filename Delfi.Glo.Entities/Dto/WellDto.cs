#pragma warning disable
using Newtonsoft.Json;

namespace Delfi.Glo.Entities.Dto
{
    public class WellDetailsDto
    {
        public List<WellDto>? WellDtos { get; set; }
        public CycleStatusCount? currentcycleStatusCount { get; set; }
        public ModeOfOperationCount? modeOfOperationCount { get; set; }
    }
    public class WellDto
    {
        public int Id { get; set; }
        public string? WellId { get; set; }
        public string WellName { get; set; }
        public string WellPriority { get; set; }
        public int GLISetPoint { get; set; }
        public int QOil { get; set; }
        public int QLiq { get; set; }
        public int Qg { get; set; }
        public int Qw { get; set; }
        public decimal Wc { get; set; }
        public double? CompressorUpTime { get; set; }
        public double? ProductionUpTime { get; set; }
        public decimal DeviceUpTime { get; set; }
        public string LastCycleStatus { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int CurrentGLISetpoint { get; set; }
        public string CurrentCycleStatus { get; set; }
        public string ApprovalMode { get; set; }
        public string ApprovalStatus { get; set; }
        public string UserId { get; set; }
        public int NoOfAlerts { get; set; }
        public string? WellStatus { get; set; }
        public string? FieldName { get; set; }
        public string? BatteryName { get; set; }
        public string? PadName { get; set; }
        public int FieldId { get; set; }
        public int BatteryId { get; set; }
        public int PadId { get; set; }
    }
    public class CycleStatusCount
    {
        public int Stabilization { get; set; }
        public int RateEstimation { get; set; }
        public int WaitingForApproval { get; set; }
     
    }

    public class ModeOfOperationCount
    {
        public int Approved { get; set; }
        public int WaitingForApproval { get; set; }
        public int Discard { get; set; }
        public int Override { get; set; }
    }
    public class PumpingDetails
    {
        public int TotalCount { get; set; }
        public int OverPumping { get; set; }
        public int OptimalPumping { get; set; }
        public int UnderPumping { get; set; }
    }

    public class WellChartDetails
    {
        public double Value { get; set; }
        public object[][]? Data { get; set; }
    }

    public class WellChartDetailsMinMax
    {
        public double Value { get; set; }
        public object[][]? Min { get; set; }
        public object[][]? Max { get; set; }
    }

    public class WellHierarchyRequest
    {
        public string SearchText { get; set; } = string.Empty;
        public List<NodeType>? SearchLevels { get; set; } = null;
    }

    public class WellHierarchResponse
    {
        public IEnumerable<Node>? Hierarchy { get; set; }
    }

    public class Node : ICloneable
    {
        public NodeType Type { get; set; }
        public string? Name { get; set; }
        public List<Node>? Children { get; set; }
        public bool IsOn { get; set; }
        public int NodeId { get; set; }
        public int? NodeParentId { get; set; }

        public object Clone()
        {
            return JsonConvert.DeserializeObject<Node>(JsonConvert.SerializeObject(this));
        }


    }
    public enum NodeType
    {
        Field,
        Battery,
        Pad,
        Wells
    }

}
#pragma warning restore
