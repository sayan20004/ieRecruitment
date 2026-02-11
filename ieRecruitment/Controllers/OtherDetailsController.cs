using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    // Final step - handles references, salary, and additional info
    public class OtherDetailsController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Load from database if needed
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(OtherDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to database (parse ReferencesJSON, mark form as complete)
                TempData["SuccessMessage"] = "Recruitment form submitted successfully! We will review your application and get back to you soon.";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
