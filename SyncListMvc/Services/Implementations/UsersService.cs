using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SyncList.CommonLibrary.Helpers;
using SyncList.CommonLibrary.Models;
using SyncListMvc.Services.Interfaces;

namespace SyncListMvc.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private IHttpRequester _httpRequester;
        private string _uri;

        private string UsersApiUri => $"{_uri}/v1/users";

        public UsersService(string uri)
        {
            _uri = uri;
            _httpRequester = new JsonHttpRequester();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _httpRequester.GetAsync<User>($"{UsersApiUri}/{id}");

            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _httpRequester.GetAsync<List<User>>(UsersApiUri);

            return users;
        }
    }
}