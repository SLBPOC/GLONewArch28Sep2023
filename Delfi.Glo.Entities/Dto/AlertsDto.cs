namespace Delfi.Glo.Entities.Dto
{
    public class AlertsDto
    {
        public int AlertId { get; set; }
        public string? WellId { get; set; }
        public string? WellName { get; set; }
        public string? AlertLevel { get; set; }
        public string? Date { get; set; }
        public string? Desc { get; set; }
        public string? Status { get; set; }
        public string? Action { get; set; }
        public bool SnoozeFlag { get; set; }
        public string? SnoozeDateTime { get; set; }
        public int SnoozeInterval { get; set; }
        public string? Comment { get; set; }
        public string? Category { get; set; }
        public string? UpdateBy { get; set; }
    }

    public class AlertCategory
    {
        public string? Name { get; set; }
        public int  Value { get; set; }
    }

    public class Alertcount
    {
        public string? Wellname { get; set; }
        public int AlertCount { get; set; }
        public int SnoozAlertCount { get; set; }
        public int Low { get; set; }
        public int High { get; set; }
        public int Medium { get; set; }
    }

    public class SnoozList
    {
        public string? Desc { get; set; }
        public int Alertid { get; set; }

        public string? Wellname { get; set; }
    }
}
