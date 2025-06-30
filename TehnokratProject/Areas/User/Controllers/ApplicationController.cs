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
            ModelState.Remove("comment");
            if (!ModelState.IsValid)
            {
                return View(model); // Повертає форму з помилками
            }
            return await send_message(model, "buy", productId);
        }

        public IActionResult Help()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Help(ContactForm model)
        {
            ModelState.Remove("comment");
            if (!ModelState.IsValid)
            {
                return View(model); // Повертає форму з помилками
            }
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
            var email = _config["Smtp:Email"] ?? Environment.GetEnvironmentVariable("SMTP_EMAIL");
            var password = _config["Smtp:Password"] ?? Environment.GetEnvironmentVariable("SMTP_PASSWORD");
            var host = _config["Smtp:Host"] ?? Environment.GetEnvironmentVariable("SMTP_HOST");
            var portString = _config["Smtp:Port"] ?? Environment.GetEnvironmentVariable("SMTP_PORT");
            int port = 587;
            if (!int.TryParse(portString, out port))
            {
                port = 587;
            }
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
