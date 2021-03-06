﻿using System.Collections.Generic;
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
                    errorMessage += $" StatusCode: {response.StatusCode}; ServiceName: {path};";
                }

                throw new HttpResponseException(response.StatusCode, errorMessage);
            }
        }
    }
}