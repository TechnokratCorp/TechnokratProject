using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TehnokratProject.Areas.Admin.ViewModels;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        ApplicationDbContext db;
        public ProductController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            var products = await db.products
                .Include(p => p.category)
                .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await db.categories.ToListAsync();
            var viewModel = new ProductCreateViewModel
            {
                Categories = categories,
                StateOptions = GetStates(),
                BrandOptions = GetBrands()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            string fileName = null;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Унікальна назва файлу
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);

                // Шлях для збереження
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products");

                // Якщо папки немає — створюємо
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                // Повний шлях до файлу
                string filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
            }

            var product = new Product
            {
                title = model.title,
                description = model.description,
                status = true,
                quantity = 0,
                image_path = "/img/products/" + fileName, // <- Шлях для фронтенду
                category_id = model.category_id.Value,
                category = db.categories.FirstOrDefault(p => p.id == model.category_id),
                price = model.price,
                state = model.state,
                brand = model.brand
            };

            db.products.Add(product);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    foreach (var state in ModelState)
            //    {
            //        foreach (var error in state.Value.Errors)
            //        {
            //            Console.WriteLine($"Field: {state.Key}, Error: {error.ErrorMessage}");
            //        }
            //    }
            //    model.BrandOptions = GetBrands();

            //    return View("Edit", model);
            //}

            var existing = await db.products.FirstOrDefaultAsync(p => p.id == model.id);
            if (existing == null) return NotFound();

            // Оновлюємо поля
            existing.title = model.title;
            existing.description = model.description;
            existing.price = model.price;
            existing.quantity = model.quantity;
            existing.status = model.status;
            existing.state = model.state;
            existing.brand = model.brand;

            // Якщо додано нове зображення
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products");
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string newFilePath = Path.Combine(uploadsFolder, newFileName);

                // Зберігаємо нове зображення
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                // Можна видалити старе зображення
                if (!string.IsNullOrEmpty(model.ExistingImagePath))
                {
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.ExistingImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Зберігаємо новий шлях
                existing.image_path = "/img/products/" + newFileName;
            }

            db.products.Update(existing);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await db.products.FirstOrDefaultAsync(p => p.id == id);
            if (product == null) return NotFound();

            var vm = new ProductUpdateViewModel
            {
                id = product.id,
                title = product.title,
                description = product.description,
                price = product.price,
                quantity = product.quantity,
                status = product.status,
                state = product.state,
                brand = product.brand,
                ExistingImagePath = product.image_path,
                BrandOptions = GetBrands(),
                StateOptions = GetStates(),
            };

            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Product? product = await db.products.FirstOrDefaultAsync(p => p.id == id);
                if (product != null)
                {
                    db.products.Remove(product);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        private List<string> GetStates()
        {
            return new List<string>
            { "Новий", "БУ", "Відновлений" };
        }

        private List<string> GetBrands()
        {
            return new List<string>
            {
                "Без бренду",
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
                "Epson", "Trust", "Xerox", 
            };
        }

    }
}
