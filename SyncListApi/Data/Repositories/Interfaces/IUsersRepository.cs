using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyncList.SyncListApi.Data.SearchOptions;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Interfaces
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        /// <summary>
        /// Gets user with joint lists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<ItemList>> GetUsersList(int userId);

        /// <summary>
        /// Searches for users by criteria
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns></returns>
        Task<List<User>> Search(UserSearchOptions searchOptions);
    }
}