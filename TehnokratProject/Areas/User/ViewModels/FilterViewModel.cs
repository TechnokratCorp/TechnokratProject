namespace TehnokratProject.Areas.User.ViewModels
{
    public class ProductFilterModel
    {
        public int? MinPrice { get; set; }= new();
        public int? MaxPrice { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public List<string> Brands { get; set; } = new();
        public List<string> States { get; set; } = new();
        public List<string> Quantity { get; set; } = new();

        public ProductFilterModel()
        {
            MaxPrice = 30000;
        }
    }

}
