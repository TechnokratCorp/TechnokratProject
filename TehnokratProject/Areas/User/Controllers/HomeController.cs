using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        ApplicationDbContext db;
        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }
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

        public IActionResult Products() // оняксцх
        {
            var products = db.products.Include(p => p.category).ToList();
            return View(products);
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
