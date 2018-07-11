using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Data.Repositories.Implementations
{
    /// <summary>
    /// </summary>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseModel, new()
    {
        protected readonly DataContext _dataContext;
        private IBaseRepository<T> _baseRepositoryImplementation;

        /// <inheritdoc />
        public abstract DbSet<T> Table { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataContext"></param>
        public BaseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <inheritdoc />
        public async Task<List<T>> GetAll(int offset = 0, int limit = int.MaxValue)
        {
            return await Table.AsNoTracking().OrderBy(t => t.Id).Skip(offset).Take(limit).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<T> Get(int id)
        {
            return await Table.AsNoTracking().SingleOrDefaultAsync(t => t.Id == id);
        }

        /// <inheritdoc />
        public async Task<T> Create(T t)
        {
            if (t == null)
                return null;

            var newUser = await Table.AddAsync(t);
            await SaveChanges();
            return newUser.Entity;
        }

        /// <inheritdoc />
        public async Task Delete(T t)
        {
            if (t == null)
                return;

            Table.Remove(t);
            await SaveChanges();
        }

        /// <inheritdoc />
        public abstract Task<T> Update(int id, T t);

        /// <inheritdoc />
        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<bool> Exists(int id)
        {
            return await Table.AsNoTracking().AnyAsync(t => t.Id == id);
        }
    }
}