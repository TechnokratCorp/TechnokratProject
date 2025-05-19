using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.ViewModels
{
    public class ProductFilterPageViewModel
    {
        public ProductFilterModel Filters { get; set; } = new();

        public List<Product> Products { get; set; } = new();

        public List<Category> Categories { get; set; }

        public List<string> Brands { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; } = 12; // Кількість товарів на сторінку
    }

}
