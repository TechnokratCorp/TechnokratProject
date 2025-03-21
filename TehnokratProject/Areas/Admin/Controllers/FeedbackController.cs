using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TehnokratProject.Data;
using TehnokratProject.Models;

namespace TehnokratProject.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        [HttpPost]
        public async Task<IActionResult> EditPost(Feedback feedbacks)
        {
            db.feedbacks.Update(feedbacks);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Feedback? feedback = await db.feedbacks.FirstOrDefaultAsync(c => c.id == id);
                if (feedback != null)
                {
                    return View(feedback);
                }
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Feedback? feedback = await db.feedbacks.FirstOrDefaultAsync(p => p.id == id);
                if (feedback != null)
                {
                    db.feedbacks.Remove(feedback);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
