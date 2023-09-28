namespace Delfi.Glo.Api.Exceptions.DataValidation
{
    public class InvalidIntegerException : Exception
    {
        public InvalidIntegerException(int integerValue) : base($"Invalid integer value {integerValue}")
        {

        }
    }
}
