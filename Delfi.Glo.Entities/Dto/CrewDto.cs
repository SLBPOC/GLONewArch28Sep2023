namespace Delfi.Glo.Entities.Dto
{
    public class CrewDto : DtoBaseEntity
    {
        public string? CrewName { get; set; }
        public UserTracking? UserTracking { get; set; }
    }
}