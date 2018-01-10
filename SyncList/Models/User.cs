using System.Collections.Generic;

namespace SyncList.SyncListApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        
        public List<ItemList> Lists { get; set; }
    }
}