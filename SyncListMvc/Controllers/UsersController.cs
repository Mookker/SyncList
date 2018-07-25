using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SyncList.CommonLibrary.Models;
using SyncListMvc.Services.Interfaces;

namespace SyncListMvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _usersService.GetUsers();
            return View(users);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            
            var user = await _usersService.GetUser(id.Value);

            if (user == null)
                return NotFound();
            
            return View(user);
        }
    }
}