using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TehnokratProject.Models;
using Microsoft.Extensions.Configuration;
using TehnokratProject.Data;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class ApplicationController : Controller
    {
        private readonly IConfiguration _config;
        private ApplicationDbContext db;
        public ApplicationController(IConfiguration config, ApplicationDbContext db_)
        {
            _config = config;
            db = db_;
        }

        public IActionResult Buy(int id)
        {
            ViewBag.ProductId = id; // Передаємо ID у View
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Buy(ContactForm model, int productId)
        {
            return await send_message(model, "buy", productId);
        }

        public IActionResult Help()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Help(ContactForm model)
        {
            return await send_message(model, "help", 0);
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> send_message(ContactForm model, string type, int productId)
        {
            var from = new MailAddress("dmitrywork33@gmail.com", "Форма з сайту"); // email smtp
            var to = new MailAddress("kurson.d@gmail.com"); // Email менеджера

            string subject = "";
            if (type == "buy")
            {
                subject = "НОВЕ ЗАМОВЛЕННЯ ТОВАРУ - Товар " + db.products.FirstOrDefault(p => p.id == productId).title + " (" + productId + ")";
            }
            else if (type == "help")
            {
                subject = "НОВА ЗАЯВКА НА РЕМОНТ";
            }
            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = $"Ім’я: {model.name}\nНомер: {model.phone}\nПовідомлення: {model.comment}"
            };
            var email = _config["Smtp:Email"];
            var password = _config["Smtp:Password"];
            var host = _config["Smtp:Host"];
            var port = int.Parse(_config["Smtp:Port"]);
            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                Credentials = new NetworkCredential(email, password)
            };

            await smtp.SendMailAsync(message);
            ViewBag.Message = "Повідомлення надіслано!";

            return RedirectToAction("Success", "Application", new { area = "User" });
        }
        
    }
}
