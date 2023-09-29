namespace Delfi.Glo.Entities.Dto
{
    public class WellListFilterDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchText { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchStatus { get; set; }
        public string[]? LastCycleStatus { get; set; }
        public string[]? CurrentCycleStatus { get; set; }
        public string[]? ModeOfOperation { get; set; }
        public string[]? ModeOfApproval { get; set; }
        public decimal[]? DeviceUpTime { get; set; }
        public double[]? CompressorUpTime { get; set; }
        public double[]? ProductionUpTime { get; set; }
        public string[]? WellNames { get; set; }
        public int[]? Ids { get; set; }
    }
    public class Range
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }
    public class IsChecked
    {
        public string? Value { get; set; }
        public bool Checked { get; set; } = false;
    }
    public class WellIdName
    {
        public string? WellId { get; set; }
        public string? WellName { get; set; }
    }
    public class DefaultWellFilterValues
    {
        public List<WellIdName>? WellNames { get; set; }
        public List<IsChecked>? CurrentCycleStatus { get; set; }
        public List<IsChecked>? LastCycleStatus { get; set; }
        public List<IsChecked>? ModeOfOperation { get; set; }
        public List<IsChecked>? ModeOfApproval { get; set; }
        public List<IsChecked>? DeviceUpTime { get; set; }
        public List<IsChecked>? CompressorUpTime { get; set; }
        public List<IsChecked>? ProductionUpTime { get; set; }

        //public Range? SpmSlider { get; set; }
        //public Range? PumpFillageSlider { get; set; }
        //public Range? InferredProductionSlider { get; set; }
        //public Range? EffectiveRuntimeSlider { get; set; }
        //public Range? CyclesTodaySlider { get; set;
        //        public List<IsChecked>? ModeOfOperation { get; set; }

        //public Range? StructuralLoadSlider { get; set; }
        //public Range? MinMaxLoadSlider { get; set; }
        //public Range? GearboxLoadSlider { get; set; }
        //public Range? RodStressSlider { get; set; }
    }
}
