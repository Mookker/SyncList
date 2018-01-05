using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyncList.Models;

namespace SyncList.Data.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        Task<List<User>> GetAllUsers(int offset = 0, int limit = Int32.MaxValue);

        /// <summary>
        /// Gets specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUser(int id);

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateUser(User user);

        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task DeleteUser(User user);

        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> UpdateUser(int id, User user);

        /// <summary>
        /// Saves changes
        /// </summary>
        /// <returns></returns>
        Task SaveChanges();
    }
}