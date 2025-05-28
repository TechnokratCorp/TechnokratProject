namespace TehnokratProject.Models
{
    public class Product
    {
        public int id { get; set; }
        public string title { get; set; }
        public string brand { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }  
        public string state { get; set; }       // БУ / НОВИЙ / ВІДНОВЛЕНИЙ
        public bool status { get; set; }        // ЧИ ВІДОБРАЖАЄТЬСЯ НА ЕКРАНІ

        public string image_path { get; set; }
        public int category_id { get; set; }
        public Category category { get; set; }
    }
}
