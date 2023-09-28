namespace Delfi.Glo.Common.Models
{
    public class EnvironmentVariables
    {
        public bool IsAuthenticationRequired { get; set; }
        public string? DbConnectionString { get; set; }

        public void ReadEnvironmentVariables()
        {
            // Postgres  databse connection
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("connectionString")))
                DbConnectionString = Environment.GetEnvironmentVariable("connectionString");

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IsAuthenticationRequired")))
                IsAuthenticationRequired = bool.Parse(Environment.GetEnvironmentVariable("IsAuthenticationRequired")!);
        }
    }
}
