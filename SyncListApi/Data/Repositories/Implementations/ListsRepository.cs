﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Implementations
{
    public class ListsRepository : BaseRepository<ItemList>, IListsRepository
    {
        protected override DbSet<ItemList> Table => _dataContext.Lists;
        
        public ListsRepository(DataContext dataContext) : base(dataContext)
        {
        }
        
        /// <inheritdoc />
        public override async Task<ItemList> Update(int id, ItemList list)
        {
            var existingList = await Table.SingleOrDefaultAsync(l => l.Id == id);
            if (existingList == null)
                return null;

            existingList.User = list.User;
            existingList.Name = list.Name;
            existingList.UserId = list.UserId;
            
            await SaveChanges();

            return existingList;
        }
    }
}