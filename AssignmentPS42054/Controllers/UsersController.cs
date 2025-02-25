using Microsoft.AspNetCore.Mvc;

namespace AssignmentPS42054.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
