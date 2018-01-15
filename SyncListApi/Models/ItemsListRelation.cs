using System;
using System.Collections.Generic;

namespace SyncList.SyncListApi.Models
{
    public partial class ItemsListRelation : BaseModel
    {
        public int ItemId { get; set; }
        public int ListId { get; set; }
        public bool IsActive { get; set; }

        public Item Item { get; set; }
        public ItemList ItemList { get; set; }
    }
}
