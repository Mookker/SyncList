using System.Collections.Generic;
using System.Threading.Tasks;
using SyncList.CommonLibrary.Models;

namespace SyncListMvc.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetUser(int id);
        Task<List<User>> GetUsers();
    }
}