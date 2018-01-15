using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Data;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemsRepository : BaseRepository<Item>, IItemsRepository
    {
        public override DbSet<Item> Table => _dataContext.Items;

        public ItemsRepository(DataContext dataContext) : base(dataContext)
        {
        }

        /// <inheritdoc />
        public override async Task<Item> Update(int id, Item item)
        {
            var existingUser = await Table.SingleOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return null;

            existingUser.Name = item.Name;
            
            await SaveChanges();

            return existingUser;
        }
        
        /// <inheritdoc />
        public async Task<List<ItemsListRelation>> GetListWithItems(int listId)
        {
//            var result = await Table.Include(l => l.ItemListRelations.
//            
//            return result;

            return null;
        }
    }
}