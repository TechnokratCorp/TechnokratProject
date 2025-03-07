namespace TehnokratProject.Models
{
    public class Feedback
    {
        public int id { get; set; }
        public string author_name { get; set; }
        public string author_email { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }
        public int rating { get; set; }
    }
}
