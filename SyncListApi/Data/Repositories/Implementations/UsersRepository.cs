using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Data.SearchOptions;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        protected override DbSet<User> Table => _dataContext.Users;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        public UsersRepository(DataContext dataContext) : base(dataContext)
        {
        }

        /// <inheritdoc />
        public override async Task<User> Update(int id, User user)
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
        public async Task<List<ItemList>> GetUsersList(int userId)
        {
            var existingUser = await Table.Include(u => u.Lists).SingleOrDefaultAsync(u => u.Id == userId);

            return existingUser?.Lists;
        }

        /// <inheritdoc />
        public async Task<List<User>> Search(UserSearchOptions searchOptions)
        {
            var users = await Table.Where(user =>
            
                (!searchOptions.Id.HasValue || user.Id == searchOptions.Id) &&
                (String.IsNullOrWhiteSpace(searchOptions.Email) || searchOptions.Email == user.Email)

            ).ToListAsync();

            return users;
        }
    }
}