using System.ComponentModel.DataAnnotations;

namespace TehnokratProject.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Введіть ім’я")]
        public string name { get; set; }

        [Required(ErrorMessage = "Введіть номер телефону")]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Формат номера телефону має бути +380XXXXXXXXX")]
        public string phone { get; set; }

        public string comment { get; set; }
    }
}
