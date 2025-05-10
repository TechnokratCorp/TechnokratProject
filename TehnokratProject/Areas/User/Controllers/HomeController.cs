using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {

        
        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Services() // оняксцх
        {
            return View();
        }

        public IActionResult About() // оняксцх
        {
            return View();
        }

        //public IActionResult Services() // оняксцх
        //{
        //    return View();
        //}

        //public IActionResult Services() // оняксцх
        //{
        //    return View();
        //}
    }
}
