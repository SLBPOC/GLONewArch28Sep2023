//disabling warning for now will fix when actual api will be consumed.
#pragma warning disable
using Delfi.Glo.Common.Models;
using Delfi.Glo.Common.Services.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Delfi.Glo.Common.Services
{
    public class HttpService<T> : IHttpService<T> where T : class
    {
        private readonly HttpClient httpClient;
        private readonly BaseUrls baseUrls;
        public HttpService(HttpClient httpClient, BaseUrls baseUrls)
        {
            this.httpClient = httpClient;
            this.baseUrls = baseUrls;
        }
        public async Task<List<T>> HttpGetAllByuId(string uri, int id, int uid)
        {
            var result = await httpClient.GetAsync($"{baseUrls.ApiBaseUrl}{uri}/{id}");
            if (!result.IsSuccessStatusCode) return null;
            return await FromHttpResponseMessageList<T>(result);
        }
        public async Task<List<T>> HttpGetAllById(string uri, int id)
        {
            var result = await httpClient.GetAsync($"{baseUrls.ApiBaseUrl}{uri}/{id}");
            if (!result.IsSuccessStatusCode) return null;
            return await FromHttpResponseMessageList<T>(result);
        }
        public async Task<T> HttpGet(string uri)
        {
            var result = await httpClient.GetAsync($"{baseUrls.ApiBaseUrl}{uri}");
            if (!result.IsSuccessStatusCode) return null;

            return await FromHttpResponseMessage<T>(result);
        }
        public async Task<List<T>> HttpGetAll(string uri)
        {
            var result = await httpClient.GetAsync($"{baseUrls.ApiBaseUrl}{uri}");
            if (!result.IsSuccessStatusCode) return null;
            return await FromHttpResponseMessageList<T>(result);
        }
        public async Task<T> HttpDelete(string uri, int id)
        {
            var result = await httpClient.DeleteAsync($"{baseUrls.ApiBaseUrl}{uri}/{id}");
            if (!result.IsSuccessStatusCode) return null;
            return await FromHttpResponseMessage<T>(result);
        }
        public async Task<T> HttpPost(string uri, object dataToSend)
        {
            var content = ToJson(dataToSend);
            var result = await httpClient.PostAsync($"{baseUrls.ApiBaseUrl}{uri}", content);
            if (!result.IsSuccessStatusCode) return null;
            return await FromHttpResponseMessage<T>(result);
        }
        public async Task<T> HttpPut(string uri, object dataToSend)
        {
            var content = ToJson(dataToSend);
            var result = await httpClient.PutAsync($"{baseUrls.ApiBaseUrl}{uri}", content);
            if (!result.IsSuccessStatusCode) return null;
            return await FromHttpResponseMessage<T>(result);
        }
        private HttpRequestException RequestException(HttpResponseMessage httpResponseMessage)
        {
            throw new HttpRequestException(string.Format(CultureInfo.InvariantCulture, httpResponseMessage.StatusCode.ToString(), new object[]
            {
                (int)httpResponseMessage.StatusCode,
                httpResponseMessage.ReasonPhrase
            }));
        }
        private StringContent ToJson(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }
        private async Task<T> FromHttpResponseMessage<T>(HttpResponseMessage result)
        {
            return JsonSerializer.Deserialize<T>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        private async Task<List<T>> FromHttpResponseMessageList<T>(HttpResponseMessage result)
        {
            return JsonSerializer.Deserialize<List<T>>(await result.Content.ReadAsStringAsync(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
#pragma warning restore