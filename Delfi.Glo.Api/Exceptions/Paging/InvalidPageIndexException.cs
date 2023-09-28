namespace Delfi.Glo.Api.Exceptions.Paging
{
    public class InvalidPageIndexException : Exception
    {
        public InvalidPageIndexException(int pageIndex) : base($"Invalid page index {pageIndex}")
        {

        }
    }
}
