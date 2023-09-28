namespace Delfi.Glo.Api.Exceptions.Paging
{
    public class InvalidPageSizeException : Exception
    {
        public InvalidPageSizeException(int pageSize) : base($"Invalid page size {pageSize}")
        {

        }
    }
}
