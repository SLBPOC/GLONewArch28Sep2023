#pragma warning disable
using Newtonsoft.Json;

namespace Delfi.Glo.Entities.Dto
{
    public class WellDetailsDto
    {
        public List<WellDto>? WellDtos { get; set; }
        public PumpingDetails? PumpingDetails { get; set; }

    }
    public class WellDto
    {
        public int Id { get; set; }
        public string? WellId { get; set; }
        public string? WellName { get; set; }
        public string? DateAndTime { get; set; }
        public string? CommStatus { get; set; }
        public string? ControllerStatus { get; set; }
        public WellChartDetails? SPM { get; set; }
        public WellChartDetails? PumpFillage { get; set; }
        public WellChartDetails? InferredProduction { get; set; }
        public WellChartDetails? EffectiveRunTime { get; set; }
        public WellChartDetails? CyclesToday { get; set; }
        public WellChartDetails? StructuralLoad { get; set; }
        public WellChartDetailsMinMax? MinMaxLoad { get; set; }
        public WellChartDetails? GearboxLoad { get; set; }
        public WellChartDetails? RodStress { get; set; }
        public int NoOfAlerts { get; set; }
        public string? WellStatus { get; set; }
        public string? FieldName { get; set; }
        public string? BatteryName { get; set; }
        public string? PadName { get; set; }
        public int FieldId { get; set; }
        public int BatteryId { get; set; }
        public int PadId { get; set; }
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
