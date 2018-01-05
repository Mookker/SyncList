using Microsoft.AspNetCore.Mvc;

namespace SyncList.Controllers
{
    public class ListsApiController : Controller
    {
        // GET
        public IActionResult GetAllLists()
        {
            return Ok();
        }
    }
}