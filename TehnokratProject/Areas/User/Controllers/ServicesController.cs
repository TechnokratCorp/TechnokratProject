using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class ServicesController:Controller
    {

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Phone()
        {
            return View();
        }

        public IActionResult Laptop()
        {
            return View();
        }

        public IActionResult Printer()
        {
            return View();
        }

        public IActionResult TV()
        {
            return View();
        }

        public IActionResult PC()
        {
            return View();
        }

        public IActionResult Tablet()
        {
            return View();
        }

    }
}
