using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Implementations
{
    public class ListsRepository : IListsRepository
    {
        private readonly DataContext _dataContext;
        public DbSet<ItemList> Table => _dataContext.Lists;
        
        public ListsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <inheritdoc />
        public async Task<List<ItemList>> GetAll(int offset = 0, int limit = Int32.MaxValue)
        {
            return await Table.AsNoTracking().OrderBy(u => u.Id).Skip(offset).Take(limit).Include(l => l.User).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<ItemList> Get(int id)
        {
            return await Table.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc />
        public async Task<ItemList> Create(ItemList list)
        {
            if(list == null)
                return null;
            
            list.CreationDate = DateTime.UtcNow;
            
            var newList = await Table.AddAsync(list);
            await SaveChanges();
            return newList.Entity;
        }

        /// <inheritdoc />
        public async Task Delete(ItemList list)
        {
            if(list == null)
                return;
            
            Table.Remove(list);
            await SaveChanges();
        }

        /// <inheritdoc />
        public async Task<ItemList> Update(int id, ItemList list)
        {
            var existingList = await Table.SingleOrDefaultAsync(u => u.Id == id);
            if (existingList == null)
                return null;

            existingList.User = list.User;
            existingList.Name = list.Name;
            existingList.UserId = list.UserId;
            
            await SaveChanges();

            return existingList;
        }

        /// <inheritdoc />
        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<bool> Exists(int id)
        {
            return await Table.AsNoTracking().AnyAsync(u => u.Id == id);
        }
    }
}