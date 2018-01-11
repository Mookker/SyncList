using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Implementations
{
    public class ItemsListRelationsRepository : BaseRepository<ItemsListRelation>, IItemsListRelationsRepository
    {
        public override DbSet<ItemsListRelation> Table => _dataContext.ItemsListRelations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        public ItemsListRelationsRepository(DataContext dataContext) : base(dataContext)
        {
        }

        /// <inheritdoc />
        public override async Task<ItemsListRelation> Update(int id, ItemsListRelation item)
        {
            var existingRelation = await Table.SingleOrDefaultAsync(t => t.Id == id);
            if (existingRelation == null)
                return null;

            existingRelation.IsActive = item.IsActive;
            existingRelation.ItemId = item.ItemId;
            existingRelation.ListId = item.ListId;
            
            await SaveChanges();

            return existingRelation;
        }
    }
}