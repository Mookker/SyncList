using System;
using System.Collections.Generic;

namespace SyncList.SyncListApi.Models
{
    public class ItemList : BaseModel
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        
        public User User { get; set; }

        public ICollection<ItemsListRelation> ItemListRelations { get; set; }
    }
}