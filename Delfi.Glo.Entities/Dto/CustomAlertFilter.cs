namespace Delfi.Glo.Entities.Dto
{
    public class CustomAlertFilter
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
    }
}
