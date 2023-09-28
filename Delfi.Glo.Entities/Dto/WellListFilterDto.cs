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
        public string[]? CommStatus { get; set; }
        public string[]? ControllerStatus { get; set; }
        public string[]? PumpingType { get; set; }
        public Range? SPM { get; set; }
        public Range? PumpFillage { get; set; }
        public Range? InferredProduction { get; set; }
        public Range? EffectiveRuntime { get; set; }
        public Range? CyclesToday { get; set; }
        public Range? StructuralLoad { get; set; }
        public Range? MinMaxLoad { get; set; }
        public Range? GearboxLoad { get; set; }
        public Range? RodStress { get; set; }
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
        public List<IsChecked>? CommStatus { get; set; }
        public List<IsChecked>? ControllerStatus { get; set; }
        public List<IsChecked>? PumpingTypes { get; set; }
        public Range? SpmSlider { get; set; }
        public Range? PumpFillageSlider { get; set; }
        public Range? InferredProductionSlider { get; set; }
        public Range? EffectiveRuntimeSlider { get; set; }
        public Range? CyclesTodaySlider { get; set; }
        public Range? StructuralLoadSlider { get; set; }
        public Range? MinMaxLoadSlider { get; set; }
        public Range? GearboxLoadSlider { get; set; }
        public Range? RodStressSlider { get; set; }
    }
}
