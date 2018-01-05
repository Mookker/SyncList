using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SyncList.Data.Repositories.Interfaces;
using SyncList.Models;

namespace SyncList.Controllers
{
    public class UsersApiControler : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public UsersApiControler(IUsersRepository usersRepository)
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
            var users = await _usersRepository.GetAllUsers();
            
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
            var user = await _usersRepository.GetUser(id);

            return Ok(user);
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
            var user = await _usersRepository.GetUser(id);
            if (user == null)
                return NotFound();
            
            await _usersRepository.DeleteUser(user);
            
            return Ok(user);
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
            if (user == null)
                return BadRequest();
            
            user = await _usersRepository.CreateUser(user);

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
            if (user == null || user.Id != id || user.Id == 0)
                return BadRequest();

            await _usersRepository.UpdateUser(id, user);

            return Ok(user);
        }
    }
}