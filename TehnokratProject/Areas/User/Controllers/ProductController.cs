using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TehnokratProject.Areas.User.ViewModels;
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

        [HttpGet]
        public IActionResult Index(ProductFilterModel filter, int page = 1, int pageSize = 12)
        {
            var query = db.products.AsQueryable();

            // Фільтри
            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.price >= filter.MinPrice.Value);
            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.price <= filter.MaxPrice.Value);
            if (filter.Categories != null && filter.Categories.Any())
                query = query.Where(p => filter.Categories.Contains(p.category.title));
            if (filter.Brands != null && filter.Brands.Any())
                query = query.Where(p => filter.Brands.Contains(p.brand));
            if (filter.Statuses.Any())
            {
                query = query.Where(p => filter.Statuses.Contains(p.status));
            }
            if (filter.States != null && filter.States.Any())
                query = query.Where(p => filter.States.Contains(p.state));

            // Підрахунок сторінок
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var products = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new ProductFilterPageViewModel
            {
                Categories = db.categories.ToList(),
                Brands = new List<string>{"Apple", "Samsung", "Xiaomi", "Huawei", "OnePlus", "Oppo", "Realme", "Sony", "Google", "Nokia",
                // Ноутбуки / Комп’ютери
                "ASUS", "Acer", "Dell", "HP", "Lenovo", "MSI", "Razer", "Microsoft",
                // Телевізори / Smart TV
                "TCL", "Hisense", "Panasonic", "Philips", "Sharp",
                // Побутова техніка
                "Bosch", "Whirlpool", "Electrolux", "Gorenje", "Beko", "Zanussi", "Miele", "Haier",
                // Аудіо / Навушники / Колонки
                "JBL", "Bose", "Sennheiser", "Beats", "Marshall", "Anker", "Bang & Olufsen",
                // Фото / Відео
                "Canon", "Nikon", "Fujifilm", "Olympus", "GoPro", "DJI",
                // Ігрова техніка
                "Nintendo", "Logitech", "HyperX", "SteelSeries", "Corsair",
                // Принтери / Периферія
                "Epson", "Trust", "A4Tech" },   
                Filters = filter,
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }


        public IActionResult Home()
        {
            return View();
        }
    }
}
