using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyncList.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        
        public List<ItemList> Lists { get; set; }
    }
}