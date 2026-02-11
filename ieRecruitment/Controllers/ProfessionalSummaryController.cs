using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    // Handles professional summary/objective text
    public class ProfessionalSummaryController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Load from database if needed
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ProfessionalSummaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to database
                TempData["SuccessMessage"] = "Professional summary saved successfully!";
                return RedirectToAction("Index", "PersonalDetails");
            }

            return View(model);
        }
    }
}
