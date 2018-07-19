using Microsoft.AspNetCore.Mvc;

namespace SyncListMvc.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}