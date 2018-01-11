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
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly DataContext _dataContext;
        public override DbSet<User> Table => _dataContext.Users;

        public UsersRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
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
    }
}