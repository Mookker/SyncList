using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Edit(int id)
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Delete(int id)
        {
            throw new System.NotImplementedException();
        }

    }
}