namespace Delfi.Glo.Entities.Dto
{
    public class AlertsDetailsDto
    {
        public List<AlertsDto>? Alerts { get; set; }
        public AlertsLevelDto? AlertsLevelDto { get; set; }
        public List<Alertcount>? Alertcount { get; set; }
        public List<AlertCategory>? Alertcategory { get; set; }
    }
}
