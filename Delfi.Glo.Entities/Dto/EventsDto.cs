namespace Delfi.Glo.Entities.Dto
{
    public class EventsDto
    {
        public int EventId { get; set; }
        public string? WellId { get; set; }
        public string? WellName { get; set; }
        public string? EventType { get; set; }
        public string? EventDescription { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
