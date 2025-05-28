namespace TehnokratProject.Models
{
    public class Solution
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int price { get; set; }
        public int problem_id { get; set; }
        public Problem problem { get; set; }

    }
}
