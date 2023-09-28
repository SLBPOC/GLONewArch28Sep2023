namespace Delfi.Glo.Api.Exceptions.DataValidation
{
    public class InvalidStringException : Exception
    {
        public InvalidStringException(string stringValue) : base($"Nullable string value {stringValue}")
        {

        }
    }
}
