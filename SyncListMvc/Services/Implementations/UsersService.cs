using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SyncList.CommonLibrary.Extensions;
using SyncList.CommonLibrary.Models;
using SyncListMvc.Constants;
using SyncListMvc.Services.Interfaces;

namespace SyncListMvc.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private HttpClient _httpClient;

        public UsersService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient(ServicesHttpClientNames.UsersService);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _httpClient.GetAsync<User>($"/{id}");

            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _httpClient.GetAsync<List<User>>("");

            return users;
        }
    }
}