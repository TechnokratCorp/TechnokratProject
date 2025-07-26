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
                .Include(p => p.Images)
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
            //if (!ModelState.IsValid)
            //{
            //    // Знову заповнюємо списки для селектів (інакше будуть null)
            //    model.BrandOptions = GetBrands();
            //    model.StateOptions = GetStates(); // Твій метод
            //    model.Categories = await db.categories.ToListAsync();
            //    model.ExistingImages = await db.ProductImages
            //        .Where(img => img.ProductId == model.id)
            //        .ToListAsync();

            //    return View(model); // Повертає форму з тими ж даними та помилками
            //}


            var product = new Product
            {
                id = model.id,
                title = model.title,
                description = model.description,
                status = true,
                quantity = 0,
                category_id = model.category_id.Value,
                category = db.categories.FirstOrDefault(p => p.id == model.category_id),
                price = model.price,
                state = model.state,
                brand = model.brand,
                Images = new List<ProductImage>()
            };

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products");

                if (!Directory.Exists(uploads))
                    Directory.CreateDirectory(uploads);

                foreach (var file in model.ImageFiles)
                {
                    if (file != null && file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(uploads, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        product.Images.Add(new ProductImage
                        {
                            ImagePath = "/img/products/" + fileName,
                            ProductId = product.id, //?
                            Product = product,    //?
                        });
                    }
                }
            }

            db.products.Add(product);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductUpdateViewModel model)
        {
            var existing = await db.products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.id == model.id);

            if (existing == null) return NotFound();

            // Оновлення властивостей
            existing.title = model.title;
            existing.description = model.description;
            existing.price = model.price;
            existing.quantity = model.quantity;
            existing.status = model.status;
            existing.state = model.state;
            existing.brand = model.brand;

            // Завантаження нових зображень (якщо є)
            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/products");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                foreach (var file in model.ImageFiles)
                {
                    if (file != null && file.Length > 0)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Додаємо нову картинку до товару
                        existing.Images.Add(new ProductImage
                        {
                            ImagePath = "/img/products/" + fileName
                        });
                    }
                }
            }

            db.products.Update(existing);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await db.products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.id == id);

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
                ExistingImagePaths = product.Images.Select(img => img.ImagePath).ToList(),
                ExistingImages = product.Images.ToList(),
                BrandOptions = GetBrands(),
                StateOptions = GetStates(),
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await db.products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.id == id);

            if (product == null)
                return NotFound();

            // 1. Видаляємо файли з сервера
            foreach (var image in product.Images)
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImagePath.TrimStart('/'));

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            // 2. Видаляємо сам товар і пов’язані зображення з БД
            db.ProductImages.RemoveRange(product.Images); // Якщо не каскадно
            db.products.Remove(product);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await db.ProductImages.FindAsync(id);
            if (image != null)
            {
                db.ProductImages.Remove(image);
                await db.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = image.ProductId });
            }

            return RedirectToAction("Index");
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
