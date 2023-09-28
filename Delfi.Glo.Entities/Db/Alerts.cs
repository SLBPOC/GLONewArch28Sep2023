namespace Delfi.Glo.Entities.Db
{
    public class Alerts
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
    }
}
