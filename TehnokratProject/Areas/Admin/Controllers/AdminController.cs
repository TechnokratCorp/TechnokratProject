using Microsoft.AspNetCore.Mvc;

namespace TehnokratProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        // CRUD - create, read, update, delete
        public IActionResult MainPage()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

      
    }
}
