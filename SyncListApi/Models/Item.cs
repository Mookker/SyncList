using System.Collections.Generic;

namespace SyncList.SyncListApi.Models
{
    public partial class Item : BaseModel
    {
        public string Name { get; set; }
        public ICollection<ItemsListRelation> ItemListRelations { get; set; }
    }
}
