using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            var query = db.products.Include(p => p.Images).AsQueryable();

            // Мінімальна ціна
            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.price >= filter.MinPrice.Value);

            // Максимальна ціна
            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.price <= filter.MaxPrice.Value);

            // Категорії
            if (filter.Categories != null && filter.Categories.Any())
                query = query.Where(p => filter.Categories.Contains(p.category.title));

            // Бренди
            if (filter.Brands != null && filter.Brands.Any())
                query = query.Where(p => filter.Brands.Contains(p.brand));

            // СТАН: "Новий", "Б/у" тощо
            if (filter.States != null && filter.States.Any())
                query = query.Where(p => filter.States.Contains(p.state));

            if (filter.Quantity != null && filter.Quantity.Any())
            {
                query = query.Where(p =>
                    (filter.Quantity.Contains("в наявності") && p.quantity > 0) ||
                    (filter.Quantity.Contains("немає в наявності") && p.quantity <= 0)
                );
            }


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
                Brands = new List<string>{"Без бренду",
                // Смартфони / Гаджети
                "Apple", "Samsung", "Xiaomi", "Huawei", "OnePlus", "Oppo", "Realme", "Sony", "Google", "Nokia",
                // Ноутбуки / Комп’ютери
                "ASUS", "Acer", "Dell", "HP", "Lenovo", "MSI", "Razer",
                // Телевізори / Smart TV
                "TCL", "Hisense", "Panasonic", "Philips", "Sharp",
                // Аудіо / Навушники / Колонки
                "JBL", "Bose", "Beats",
                // Ігрова техніка
                "Nintendo", "Logitech", "HyperX", "Corsair",
                // Принтери / Периферія
                "Epson", "Trust", "Xerox",  },   
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
