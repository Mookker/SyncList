using System.Collections.Generic;

namespace SyncList.SyncListApi.Models
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        
        public List<ItemList> Lists { get; set; }

        /// <summary>
        /// Checks if users are the same
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Equals(User obj)
        {
            return Id == obj.Id && Email == obj.Email && Name == obj.Name;
        }
    }
}