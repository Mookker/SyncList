using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _dataContext;
        public DbSet<User> Table => _dataContext.Users;

        public UsersRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <inheritdoc />
        public async Task<List<User>> GetAll(int offset = 0, int limit = Int32.MaxValue)
        {
            return await Table.AsNoTracking().OrderBy(u => u.Id).Skip(offset).Take(limit).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User> Get(int id)
        {
            return await Table.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc />
        public async Task<User> Create(User user)
        {
            if(user == null)
                return null;
                
            var newUser = await Table.AddAsync(user);
            await SaveChanges();
            return newUser.Entity;
        }

        /// <inheritdoc />
        public async Task Delete(User user)
        {
            if(user == null)
                return;
            
            Table.Remove(user);
            await SaveChanges();
        }

        /// <inheritdoc />
        public async Task<User> Update(int id, User user)
        {
            var existingUser = await Table.SingleOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return null;

            existingUser.Email = user.Email;
            existingUser.Name = user.Name;
            
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