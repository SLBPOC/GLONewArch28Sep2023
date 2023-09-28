using System.Diagnostics.CodeAnalysis;

namespace Delfi.Glo.Entities.Dto
{
    public class CustomAlertDetailsDto : DtoBaseEntity
    {
        public List<CustomAlertDto>? CustomAlertDto { get; set; }
        public CountDetails? CountDetails { get; set; }
        [AllowNull]
        public List<WellFilterListDetails> WellFilterListDetails { get; set; }
    }

    public class CustomAlertDto
    {
        public int Id { get; set; }
        public string? WellName { get; set; }
        public string? CustomAlertName { get; set; }
        public string? NotificationType { get; set; }
        public string? Priority { get; set; }
        public string? Category { get; set; }
        public string? Operator { get; set; }
        public string? Value { get; set; }
        public int? ActualValue { get; set; }
        public bool IsActive { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }

    public class CountDetails
    {
        public int TotalCount { get; set; }
    }

    public class WellFilterListDetails
    {
        public string? WellId { get; set; }
        public string? WellName { get; set; }
    }
}