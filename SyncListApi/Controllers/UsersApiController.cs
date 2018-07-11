using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyncList.CommonLibrary.Validation;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;

namespace SyncList.SyncListApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class UsersApiController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usersRepository"></param>
        public UsersApiController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/users")]
        public async Task<IActionResult> GetAllUsers([FromQuery]int offset = 0, [FromQuery]int limit = Int32.MaxValue)
        {
            Validator.Assert(offset >= 0 && limit >= 0, ValidationAreas.InputParameters);
            
            var users = await _usersRepository.GetAll();
            
            return Ok(users);
        }

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _usersRepository.Get(id);
            
            Validator.Assert(user != null, ValidationAreas.NotExists);
            
            return Ok(user);
        }
        
        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/v1/users/{id}/lists")]
        public async Task<IActionResult> GetUserList(int id)
        {
            var lists = await _usersRepository.GetUsersList(id);
            
            Validator.Assert(lists != null, ValidationAreas.NotExists);
            
            return Ok(lists);
        }
        
        /// <summary>
        /// Deletes user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/v1/users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _usersRepository.Get(id);
            
            Validator.Assert(user != null, ValidationAreas.NotExists);

            await _usersRepository.Delete(user);
            
            return Ok();
        }
        
        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/v1/users/")]
        public async Task<IActionResult> CreateUser([FromBody]User user)
        {
            Validator.Assert(user != null, ValidationAreas.InputParameters);
            var users = await _usersRepository.Search(new UserSearchOptions
            {
                Email = user.Email
            });
            
            Validator.Assert(users?.Any() != true, ValidationAreas.AlreadyExists);
            
            user = await _usersRepository.Create(user);

            return Ok(user);
        }
        
        /// <summary>
        /// Updates or creates user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/v1/users/{id}")]
        public async Task<IActionResult> UpdateOrCreateUser([FromRoute] int id, [FromBody]User user)
        {
            Validator.Assert(user != null && user.Id == id && user.Id != 0, ValidationAreas.InputParameters);

            var exists = await _usersRepository.Exists(id);
            if (exists)
            {
                await _usersRepository.Update(id, user);
            }
            else
            {
                await _usersRepository.Create(user);
            }
            
            return Ok(user);
        }
    }
}