namespace TehnokratProject.Models
{
    public class Problem
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int category_id { get; set; }
        public List<Solution> solutions { get; set; }
        public Category category { get; set; }
    }
}
