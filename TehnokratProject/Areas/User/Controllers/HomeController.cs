using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        ApplicationDbContext db;
        public HomeController(IConfiguration config, ApplicationDbContext context)
        {
            db = context;
            _config = config;
        }
        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Services() // ÏÎÑËÓÃÈ
        {
            return View();
        }

        public IActionResult About() // ÏÎÑËÓÃÈ
        {
            return View();
        }

        public IActionResult Products() // ÏÎÑËÓÃÈ
        {
            var products = db.products.Include(p => p.category).ToList();
            return View(products);
        }

        //public IActionResult Services() // ÏÎÑËÓÃÈ
        //{
        //    return View();
        //}

        //public IActionResult Services() // ÏÎÑËÓÃÈ
        //{
        //    return View();
        //}
        [HttpPost]
        public async Task<IActionResult> send_message(ContactForm model)
        {
            ModelState.Remove("comment");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Contacts");
            }
            var from = new MailAddress("dmitrywork33@gmail.com", "Ôîğìà ç ñàéòó"); // email smtp
            var to = new MailAddress("kurson.d@gmail.com"); // Email ìåíåäæåğà

            string subject = "";
            subject = "ÍÎÂÀ ÇÀßÂÊÀ ÍÀ ĞÅÌÎÍÒ";
            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = $"²ì’ÿ: {model.name}\nÍîìåğ: {model.phone}\nÏîâ³äîìëåííÿ: {model.comment}"
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
            ViewBag.Message = "Ïîâ³äîìëåííÿ íàä³ñëàíî!";

            return RedirectToAction("Success", "Application", new { area = "User" });
        }
    }
}
