namespace Delfi.Glo.Entities.Dto
{
    public class EventDetailsDto
    {
        public int Totalcount { get; set; }
        public List<EventsDto>? Events { get; set; }
    }
   
    public class EventTypes
    {
        private readonly string[] ArrayEventTypes = { "DynaCard", "Mitigation", "Algorithm", "Controller", "Alerts", "System" };
        public string[] EventTypeList
        {
            get { return ArrayEventTypes; }
        }
    }

}
