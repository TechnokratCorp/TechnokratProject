using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.Admin.ViewModels
{
    public class ProductCreateViewModel
    {
        [BindNever]
        public IEnumerable<Category> Categories { get; set; }
        public string title { get; set; }
        public int? category_id { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public bool status { get; set; }
        public string image_path { get; set; }

        // Завантажене зображення
        [Display(Name = "Зображення")]
        public IFormFile ImageFile { get; set; }

        public ProductCreateViewModel()
        {
            Categories = Enumerable.Empty<Category>();
        }
    }
}
