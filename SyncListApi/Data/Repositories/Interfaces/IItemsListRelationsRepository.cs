using System.Collections.Generic;
using System.Threading.Tasks;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Interfaces
{
    /// <inheritdoc />
    public interface IItemsListRelationsRepository : IBaseRepository<ItemsListRelation>
    {
        /// <summary>
        /// Gets filled list of list + items
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        Task<ItemsListRelation> GetListWithItems(int listId);
    }
}