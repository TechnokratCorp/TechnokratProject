using Microsoft.AspNetCore.Mvc;

namespace TehnokratProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromblemController : Controller
    {
        // CRUD - create, read, update, delete
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        //public IActionResult FeedbackCreate()
        //{

        //}
    }
}
