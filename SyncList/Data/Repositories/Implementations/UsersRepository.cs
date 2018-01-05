using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.Data.Repositories.Interfaces;
using SyncList.Models;

namespace SyncList.Data.Repositories.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _dataContext;

        public UsersRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <inheritdoc />
        public async Task<List<User>> GetAllUsers(int offset = 0, int limit = Int32.MaxValue)
        {
            return await _dataContext.Users.AsNoTracking().OrderBy(u => u.Id).Skip(0).Take(limit).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User> GetUser(int id)
        {
            return await _dataContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc />
        public async Task<User> CreateUser(User user)
        {
            if(user == null)
                return null;
                
            var newUser = await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            return newUser.Entity;
        }

        /// <inheritdoc />
        public async Task DeleteUser(User user)
        {
            if(user == null)
                return;
            
             _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<User> UpdateUser(int id, User user)
        {
            var existingUser = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (existingUser == null)
                return await CreateUser(user);

            existingUser.Email = user.Email;
            existingUser.Name = user.Name;
            
            await _dataContext.SaveChangesAsync();

            return existingUser;
        }

        /// <inheritdoc />
        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}