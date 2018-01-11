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
    public class ItemsRepository : IItemsRepository
    {
        private readonly DataContext _dataContext;
        public DbSet<Item> Table => _dataContext.Items;

        public ItemsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <inheritdoc />
        public async Task<List<Item>> GetAll(int offset = 0, int limit = Int32.MaxValue)
        {
            return await Table.AsNoTracking().OrderBy(u => u.Id).Skip(offset).Take(limit).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Item> Get(int id)
        {
            return await Table.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc />
        public async Task<Item> Create(Item item)
        {
            if(item == null)
                return null;
                
            var newUser = await Table.AddAsync(item);
            await SaveChanges();
            return newUser.Entity;
        }

        /// <inheritdoc />
        public async Task Delete(Item item)
        {
            if(item == null)
                return;
            
            Table.Remove(item);
            await SaveChanges();
        }

        /// <inheritdoc />
        public async Task<Item> Update(int id, Item item)
        {
            var existingUser = await Table.SingleOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return null;

            existingUser.Name = item.Name;
            
            await SaveChanges();

            return existingUser;
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