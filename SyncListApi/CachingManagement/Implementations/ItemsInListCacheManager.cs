using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SyncList.CommonLibrary.Extensions;
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
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="distributedCache"></param>
        public ItemsInListCacheManager(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        
        /// <inheritdoc />
        public async Task<ListWithItemsCache> AddList(ItemList list)
        {
            var listWithItemsCache = new ListWithItemsCache() {Id = list.Id, Name = list.Name};
            await _distributedCache.TrySetCacheAsync(CreateCacheKey(list.Id), listWithItemsCache,
                CreateCacheOptions());

            return listWithItemsCache;
        }

        public async Task AddList(ListWithItemsCache list)
        {
            await _distributedCache.TrySetCacheAsync(CreateCacheKey(list.Id), list,
                CreateCacheOptions());

        }

        /// <inheritdoc />
        public async Task<bool> AddItemToList(int listId, CachedItem item)
        {
            var list = await GetList(listId);
            if (list == null)
                return false;
            
            list.Items.Add(item);
            await _distributedCache.TrySetCacheAsync(CreateCacheKey(list.Id), list, CreateCacheOptions());

            return true;
        }

        /// <inheritdoc />
        public async Task<ListWithItemsCache> GetList(int listId)
        {
            var list = await _distributedCache.TryGetCacheAsync<ListWithItemsCache>(CreateCacheKey(listId));

            return list;
        }

        private static string CreateCacheKey(int listId)
        {
            return $"list:{listId}";
        }

        private static DistributedCacheEntryOptions CreateCacheOptions()
        {
            return new DistributedCacheEntryOptions() {AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)};
        }
    }
}