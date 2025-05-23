using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class Application : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ContactForm model)
        {
            var from = new MailAddress("dmitrywork33@gmail.com", "Форма з сайту"); // email smtp
            var to = new MailAddress("kurson.d@gmail.com"); // Email менеджера
            var message = new MailMessage(from, to)
            {
                Subject = "Нова анкета з сайту",
                Body = $"Ім’я: {model.name}\nНомер: {model.phone}\nПовідомлення: {model.comment}"
            };

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // Наприклад: smtp.gmail.com
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("dmitrywork33@gmail.com", "oncbyyvwbqrttpab")
            };

            smtp.Send(message);
            ViewBag.Message = "Повідомлення надіслано!";

            return RedirectToAction("Success", "Application", new { area = "User" });
            //feedback.date = DateTime.Now;
            //if (feedback.rating > 5 || feedback.rating < 0)
            //{
            //    return RedirectToAction("Index");
            //}
            //db.feedbacks.Add(feedback);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");
        }
        public IActionResult Success()
        {
            return View();
        }

        
    }
}
