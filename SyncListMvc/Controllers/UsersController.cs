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

        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            
            var user = await _usersService.GetUser(id.Value);

            if (user == null)
                return NotFound();
            
            return View(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
//                try
//                {
//                    _context.Update(user);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!MovieExists(user.ID))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}