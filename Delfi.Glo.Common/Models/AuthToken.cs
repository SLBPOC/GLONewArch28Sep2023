using Newtonsoft.Json;

namespace Delfi.Glo.Common.Models
{
    public class AuthToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string? Scope { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string? TokenType { get; set; }

        public bool IsValidAndNotExpiring
        {
            get => !string.IsNullOrEmpty(AccessToken) && ExpiresAt > DateTime.UtcNow.AddSeconds(60);
        }
    }
}
