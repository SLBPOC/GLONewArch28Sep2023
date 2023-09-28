using System.Text.Json.Serialization;

namespace Delfi.Glo.Entities.Dto
{
    public class UniversitiesDto : DtoBaseEntity
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("alpha_two_code")]
        public string? AlphaTwoCode { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }
    }
}
