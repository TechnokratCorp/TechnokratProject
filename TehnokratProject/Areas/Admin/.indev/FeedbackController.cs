using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.User.Controllers
{
    [Area("User")]
    public class FeedbackController : Controller
    {
        ApplicationDbContext db;

        public FeedbackController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Feedback> feedbacks = await db.feedbacks.ToListAsync();
            return View(feedbacks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Feedback feedback)
        {
            feedback.date = DateTime.Now;
            if (feedback.rating > 5 || feedback.rating < 0)
            {
                return RedirectToAction("Index");
            }
            db.feedbacks.Add(feedback);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
