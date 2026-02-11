using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    // Handles family members, emergency contacts, and languages
    public class FamilyDetailsController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Load from database if needed
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FamilyDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to database (parse JSON arrays)
                TempData["SuccessMessage"] = "Family details saved successfully!";
                return RedirectToAction("Index", "EducationalDetails");
            }

            return View(model);
        }
    }
}
