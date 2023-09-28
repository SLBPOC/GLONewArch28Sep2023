namespace Delfi.Glo.Entities.Dto
{
    public class EventListFilterDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchText { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchStatus { get; set; }
        public DateRange? DateRange { get; set; }
        public string[]? WellNames { get; set; }
        public string[]? EventTypes { get; set; }
        public int[]? Ids { get; set; }
    }
}
