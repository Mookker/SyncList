using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncList.CommonLibrary.Exceptions;
using SyncList.CommonLibrary.Extensions;

namespace SyncList.CommonLibrary.Helpers
{
    public interface IHttpRequester
    {
        Task<R> GetAsync<R>(string uri) where R : class, new ();
    }

    public class JsonHttpRequester : IHttpRequester
    {
        private HttpClient _httpClient;
        public JsonHttpRequester()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
        }
        
        public async Task<R> GetAsync<R>(string uri) where R : class, new ()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                response.CheckStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<R>(responseBody);

                return responseObj;
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServiceException(ex);
            }
        }
    }
}