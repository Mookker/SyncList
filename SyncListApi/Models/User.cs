using System.Collections.Generic;

namespace SyncList.SyncListApi.Models
{
    public class User : BaseModel
    {
        protected bool Equals(User other)
        {
            return string.Equals(Email, other.Email) && string.Equals(Name, other.Name) && Equals(Lists, other.Lists);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Email != null ? Email.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Lists != null ? Lists.GetHashCode() : 0);
                return hashCode;
            }
        }

        public string Email { get; set; }
        public string Name { get; set; }
        
        public List<ItemList> Lists { get; set; }
    }
}