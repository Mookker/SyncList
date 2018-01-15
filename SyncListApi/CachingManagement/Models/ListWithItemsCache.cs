using System.Collections.Generic;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.CachingManagement.Models
{
    /// <summary>
    /// Used for caching main info
    /// </summary>
    public class ListWithItemsCache
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ListWithItemsCache()
        {
            Items = new List<CachedItem>();
        }
        
        /// <summary>
        /// List id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// List name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Items in the list
        /// </summary>
        public List<CachedItem> Items { get; set; }
    }
}