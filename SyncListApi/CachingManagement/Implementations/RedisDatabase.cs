using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using SyncList.SyncListApi.CachingManagement.Interfaces;

namespace SyncList.SyncListApi.CachingManagement.Implementations
{
    /// <summary>
    /// Implementation of wrapper
    /// </summary>
    public class RedisDatabase : IRedisDatabase
    {
        private readonly ILogger<RedisDatabase> _logger;
        private readonly string _prefix;
        private readonly IDatabase _database;

        public RedisDatabase(IDatabase database, ILogger<RedisDatabase> logger, string prefix)
        {
            _logger = logger;
            _database = database;
            _prefix = prefix ?? "";
        }

        /// <inheritdoc />
        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiration)
        {
            try
            {
                return await _database.StringSetAsync(_prefix + key, value, expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"Error in setting {_prefix + key}");
                return false;
            }

        }

        /// <inheritdoc />
        public async Task<string> GetStringAsync(string key)
        {
            try
            {
                return await _database.StringGetAsync(_prefix + key);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"Error in getting {_prefix + key}");
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan? expiration) where T : class, new()
        {
            try
            {
                _logger.LogInformation($"Setting {key}");
                if (value != null)
                    return await SetStringAsync(_prefix + key, JsonConvert.SerializeObject(value), expiration);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"Error in setting {_prefix + key}");
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<T> GetObjectAsync<T>(string key) where T : class, new()
        {
            try
            {
                var cachedString = await GetStringAsync(_prefix + key);

                if (!string.IsNullOrWhiteSpace(cachedString))
                {
                    var serializedResult = JsonConvert.DeserializeObject<T>(cachedString);

                    if (serializedResult != null)
                        return serializedResult;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"Error in getting {_prefix + key}");
            }

            return default(T);
        }

        /// <inheritdoc />
        public async Task<bool> AddToGeoAsync(string key, string value, double longitude, double latitude)
        {
            try
            {
                return await _database.GeoAddAsync(_prefix + key, longitude, latitude, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"Error in adding geo {_prefix + key}");
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<List<string>> SortByGeoAsync(string key
            , double longitude
            , double latitude
            , double rangeinKm
            , int offset
            , int limit)
        {
            try

            {
                var result = await _database.GeoRadiusAsync(_prefix + key, longitude, latitude, rangeinKm,
                    GeoUnit.Kilometers);
                return result.Select(r => r.ToString()).Skip(offset).Take(limit).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"Error in getting geo data {_prefix + key}");
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(string key)
        {
            return await _database.KeyDeleteAsync(_prefix + key);
        }

        /// <inheritdoc />
        public async Task<bool> SetHashField(string hashName, string fieldName, string value)
        {
            return await _database.HashSetAsync(_prefix + hashName, fieldName, value);
        }

        /// <inheritdoc />
        public async Task<string> GetHashField(string hashName, string fieldName)
        {
            return await _database.HashGetAsync(_prefix + hashName, fieldName);
        }

        /// <inheritdoc />
        public async Task<ExpandoObject> GetHash(string hashName)
        {
            var hash = await _database.HashGetAllAsync(_prefix + hashName);
            var dictionary = hash.ToStringDictionary();

            var result = new ExpandoObject();
            foreach (var key in dictionary.Keys)
            {
                result.TryAdd(key, dictionary[key]);
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<long> IncHashField(string hashName, string fieldName, long value)
        {
            return await _database.HashIncrementAsync(_prefix + hashName, fieldName, value);
        }

        /// <inheritdoc />
        public async Task<List<T>> GetListAsync<T>(string key, long offset, long limit) where T : class, new()
        {
            var list = await _database.ListRangeAsync(_prefix + key, offset, limit == Int32.MaxValue ? -1 : limit);
            return list.Select(i => {
                if (i.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(i.ToString());
                }

                return default(T);
            }).ToList();
        }

        /// <inheritdoc />
        public async Task SetListAsync<T>(string key, IEnumerable<T> list) where T : class, new()
        {
            while (await _database.ListLengthAsync(key) > 0)
            {
                await _database.ListRemoveAsync(key, 0);
            }

            foreach (var item in list)
            {
                await _database.ListLeftPushAsync(key, JsonConvert.SerializeObject(item));
            }
        }
    }
}