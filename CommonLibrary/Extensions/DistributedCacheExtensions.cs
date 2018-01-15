using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace SyncList.CommonLibrary.Extensions
{
    /// <summary>
    /// DistributedCache Extensions Class
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>T</returns>
        public static async Task<T> TryGetCacheAsync<T>(this IDistributedCache distributedCache, string cacheKey)
        {
            try
            {
                var cachedString = await distributedCache.GetStringAsync(cacheKey);

                if (!string.IsNullOrWhiteSpace(cachedString))
                {
                    var serializedResult = JsonConvert.DeserializeObject<T>(cachedString);

                    if (serializedResult != null)
                        return serializedResult;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception in getting {cacheKey} from cache" + ex.Message + ex.StackTrace);
            }
            return default(T);
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>T</returns>
        public static async Task<string> TryGetStringAsync(this IDistributedCache distributedCache, string cacheKey)
        {
            try
            {
                return await distributedCache.GetStringAsync(cacheKey);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in getting {cacheKey} from cache" + ex.Message + ex.StackTrace);
            }
            return default(string);
        }

        /// <summary>
        /// Sets the string is not in cache asynchronous.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        public static async Task TrySetStringAsync(this IDistributedCache distributedCache, string cacheKey, string value, DistributedCacheEntryOptions options)
        {
            try
            {
                Console.WriteLine($"Setting {cacheKey}");
                if(!String.IsNullOrWhiteSpace(value))
                    await distributedCache.SetStringAsync(cacheKey, value, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in setting {cacheKey}" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// Sets the string is not in cache asynchronous.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The options.</param>
        public static async Task TrySetCacheAsync<T>(this IDistributedCache distributedCache, string cacheKey, T value, DistributedCacheEntryOptions options)
        {
            try
            {
                Console.WriteLine($"Setting {cacheKey}");
                if(value != null)
                    await distributedCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(value), options);
            }
            catch(Exception ex)
            {

                Console.WriteLine($"Exception in setting {cacheKey}" + ex.Message + ex.StackTrace);
            }
        }

        public static async Task TrySetLongAsync(this IDistributedCache distributedCache, string cacheKey, long value, DistributedCacheEntryOptions options)
        {
            try
            {
                Console.WriteLine($"Setting {cacheKey}");
                await distributedCache.SetStringAsync(cacheKey, value.ToString(), options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in setting {cacheKey}" + ex.Message + ex.StackTrace);
            }
        }
        
        /// <summary>
        /// Gets long value from cache
        /// </summary>
        /// <param name="distributedCache"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static async Task<long?> TryGetLongAsync(this IDistributedCache distributedCache, string cacheKey)
        {
            try
            {
                var str = await distributedCache.GetStringAsync(cacheKey);
                if (!String.IsNullOrWhiteSpace(str))
                {
                    
                    var success = long.TryParse(str, out long result);
                    if (success)
                        return result;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in getting {cacheKey} from cache"+ ex.Message + ex.StackTrace);
            }
            return null;
        }
    }
}
