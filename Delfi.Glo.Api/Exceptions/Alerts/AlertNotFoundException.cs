namespace Delfi.Glo.Api.Exceptions.Alerts
{
    public class AlertNotFoundException : Exception
    {
        public AlertNotFoundException() : base($"No alert found")
        {
        }
    }
}
