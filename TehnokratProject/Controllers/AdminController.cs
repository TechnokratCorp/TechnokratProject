using Microsoft.AspNetCore.Mvc;

namespace TehnokratProject.Controllers
{
    public class AdminController : Controller
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
