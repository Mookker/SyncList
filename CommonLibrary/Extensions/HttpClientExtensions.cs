using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncList.CommonLibrary.Exceptions;

namespace SyncList.CommonLibrary.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<R> GetAsync<R>(this HttpClient _httpClient, string uri) where R : class, new ()
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