namespace SyncList.SyncListApi.Data.Repositories.Interfaces
{
    /// <summary>
    /// User search options
    /// </summary>
    public class UserSearchOptions
    {
        /// <summary>
        /// User id
        /// </summary>
        public int? Id { get; set; }
        
        
        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }
    }
}