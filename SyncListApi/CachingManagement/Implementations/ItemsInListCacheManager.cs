using System;
using System.Threading.Tasks;
using SyncList.SyncListApi.CachingManagement.Interfaces;
using SyncList.SyncListApi.CachingManagement.Models;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.CachingManagement.Implementations
{
    /// <summary>
    /// Implementation
    /// </summary>
    public class ItemsInListCacheManager : IItemsInListCacheManager
    {
        private readonly IRedisDatabase _redisDatabase;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="redisDatabase"></param>
        public ItemsInListCacheManager(IRedisDatabase redisDatabase)
        {
            _redisDatabase = redisDatabase;
        }
        
        /// <inheritdoc />
        public async Task<ListWithItemsCache> AddList(ItemList list)
        {
            var listWithItemsCache = new ListWithItemsCache() {Id = list.Id, Name = list.Name};
            await _redisDatabase.SetObjectAsync(CreateCacheKey(list.Id), listWithItemsCache,TimeSpan.FromHours(1));

            return listWithItemsCache;
        }

        /// <inheritdoc />
        public async Task AddList(ListWithItemsCache list)
        {
            await _redisDatabase.SetObjectAsync(CreateCacheKey(list.Id), list, TimeSpan.FromHours(1));

        }

        /// <inheritdoc />
        public async Task<bool> AddItemToList(int listId, CachedItem item)
        {
            var list = await GetList(listId);
            if (list == null)
                return false;
            
            list.Items.Add(item);
            await _redisDatabase.SetObjectAsync(CreateCacheKey(list.Id), list, TimeSpan.FromHours(1));

            return true;
        }

        /// <inheritdoc />
        public async Task<ListWithItemsCache> GetList(int listId)
        {
            var list = await _redisDatabase.GetObjectAsync<ListWithItemsCache>(CreateCacheKey(listId));

            return list;
        }

        private static string CreateCacheKey(int listId)
        {
            return $"list:{listId}";
        }
    }
}