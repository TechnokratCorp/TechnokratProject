using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TehnokratProject.Models;
using Microsoft.Extensions.Configuration;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class Application : Controller
    {
        private readonly IConfiguration _config;

        public Application(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Buy()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Buy(ContactForm model)
        {
            return await send_message(model, "buy");
        }

        public IActionResult Help()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Help(ContactForm model)
        {
            return await send_message(model, "help");
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> send_message(ContactForm model, string type)
        {
            var from = new MailAddress("dmitrywork33@gmail.com", "Форма з сайту"); // email smtp
            var to = new MailAddress("kurson.d@gmail.com"); // Email менеджера

            string subject = "";
            if (type == "buy")
            {
                subject = "НОВЕ ЗАМОВЛЕННЯ ТОВАРУ";
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
