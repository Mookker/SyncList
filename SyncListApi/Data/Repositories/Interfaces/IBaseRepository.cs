using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseModel, new()
    {
        /// <summary>
        /// DbSet
        /// </summary>
        DbSet<T> Table { get; }
            
        /// <summary>
        /// Gets all item
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAll(int offset = 0, int limit = Int32.MaxValue);

        /// <summary>
        /// Gets specific item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> Get(int id);

        /// <summary>
        /// Creates item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<T> Create(T item);

        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task Delete(T item);

        /// <summary>
        /// Updates item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<T> Update(int id, T item);

        /// <summary>
        /// Saves changes
        /// </summary>
        /// <returns></returns>
        Task SaveChanges();

        /// <summary>
        /// Checks if item is in db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Exists(int id);
    }
}