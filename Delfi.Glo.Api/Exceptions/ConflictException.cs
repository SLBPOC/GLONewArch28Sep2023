using System.Net;

namespace Delfi.Glo.Api.Exceptions
{
    public class ConflictException : CustomException
    {
        public ConflictException(string message)
            : base(message, null, HttpStatusCode.Conflict) { }
    }
}
