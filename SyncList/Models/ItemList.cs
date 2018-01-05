using System;

namespace SyncList.Models
{
    public class ItemList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        
        public User User { get; set; }
    }
}