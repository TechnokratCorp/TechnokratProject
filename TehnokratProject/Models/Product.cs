namespace TehnokratProject.Models
{
    public class Product
    {
        public int id { get; set; }
        public string title { get; set; }
        public int category_id { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public bool status { get; set; }
        public string image_path { get; set; }
        public Category category { get; set; }
    }
}
