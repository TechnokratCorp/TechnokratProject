namespace TehnokratProject.Models
{
    public class Category
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<Problem> problems { get; set; }
        public List<Product> products { get; set; }
    }
}
