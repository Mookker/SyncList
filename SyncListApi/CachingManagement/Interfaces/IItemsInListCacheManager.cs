using System.Threading.Tasks;
using SyncList.SyncListApi.CachingManagement.Models;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.CachingManagement.Interfaces
{
    /// <summary>
    /// Caching manager for filled list 
    /// </summary>
    public interface IItemsInListCacheManager
    {
        /// <summary>
        /// Adds list to cache
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<ListWithItemsCache> AddList(ItemList list);

        /// <summary>
        /// Adds list to cache
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task AddList(ListWithItemsCache list);

        /// <summary>
        /// Adds item for proper list
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="item"></param>
        /// <returns>True if added. False if not, or list is not cached before</returns>
        Task<bool> AddItemToList(int listId, CachedItem item);
        
        /// <summary>
        /// Gets list with items
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        Task<ListWithItemsCache> GetList(int listId);
    }
}