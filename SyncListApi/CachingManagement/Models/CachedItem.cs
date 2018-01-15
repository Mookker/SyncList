using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.CachingManagement.Models
{
    public class CachedItem
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public CachedItem()
        {
            
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isActive"></param>
        public CachedItem(Item item, bool isActive)
        {
            Id = item.Id;
            Name = item.Name;
            IsActive = isActive;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Is item active in list
        /// </summary>
        public bool IsActive { get; set; }
    }
}