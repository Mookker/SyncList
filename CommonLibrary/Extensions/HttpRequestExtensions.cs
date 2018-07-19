using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using SyncList.CommonLibrary.Exceptions;

namespace SyncList.CommonLibrary.Extensions
{
    public static class HttpRequestExtensions
    {
        public static void CheckStatusCode(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = null;

                if (response.RequestMessage!= null && !string.IsNullOrWhiteSpace(response.RequestMessage.RequestUri?.AbsolutePath))
                {
                    var path = response.RequestMessage.RequestUri.AbsolutePath.TrimStart('/');
                    var segments = path.Split('/');
                    var serviceName = segments.Length > 0 ? segments[1] : string.Empty;
                    errorMessage += $" StatusCode: {response.StatusCode}; ServiceName: {serviceName};";
                }

                throw new HttpResponseException(response.StatusCode, errorMessage);
            }
        }
    }
}