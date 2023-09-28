using System.Collections.Generic;
using System.Threading.Tasks;

namespace Delfi.Glo.Common.Services.Interfaces
{
    public interface IHttpService<T> where T : class
    {
        public Task<T> HttpGet(string uri);
        public Task<List<T>> HttpGetAll(string uri);
        public Task<T> HttpDelete(string uri, int id);
        public Task<T> HttpPost(string uri, object dataToSend);
        public Task<T> HttpPut(string uri, object dataToSend);
    }
}