using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class ProductController : Controller
    {
        ApplicationDbContext db;

        public ProductController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await db.products.ToListAsync();
            return View(products);
        }
        public IActionResult Home()
        {
            return View();
        }
    }
}
