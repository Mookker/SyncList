using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace SyncList.SyncListApi.CachingManagement.Interfaces
{
    /// <summary>
    /// Wrapper on redis db
    /// </summary>
    public interface IRedisDatabase
    {
        /// <summary>
        /// Set string
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        Task<bool> SetStringAsync(string key, string value, TimeSpan? expiration);

        /// <summary>
        /// Gets string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetStringAsync(string key);

        /// <summary>
        /// Set object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan? expiration) where T : class, new();

        /// <summary>
        /// Gets object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetObjectAsync<T>(string key) where T : class, new();

        /// <summary>
        /// Adds item to geospatial index
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        Task<bool> AddToGeoAsync(string key, string value, double longitude, double latitude);
        
        /// <summary>
        /// Sorts by geo
        /// </summary>
        /// <param name="key"></param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="rangeinKm"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<List<string>> SortByGeoAsync(string key, double longitude, double latitude, double rangeinKm, int offset, int limit);

        /// <summary>
        /// Removes key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Sets field in object
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetHashField(string hashName, string fieldName, string value);

        /// <summary>
        /// Gets field from object
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        Task<string> GetHashField(string hashName, string fieldName);

        /// <summary>
        /// Gets full hash
        /// </summary>
        /// <param name="hashName"></param>
        /// <returns></returns>
        Task<ExpandoObject> GetHash(string hashName);
        
        /// <summary>
        /// Increments object field
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> IncHashField(string hashName, string fieldName, long value);

        /// <summary>
        /// Gets list of items from cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<List<T>> GetListAsync<T>(string key, long offset, long limit) where T : class, new();

        /// <summary>
        /// Savs list of items
        /// </summary>
        /// <param name="key"></param>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task SetListAsync<T>(string key, IEnumerable<T> list) where T : class, new();
    }
}