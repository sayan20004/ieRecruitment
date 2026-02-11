using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    // Handles academic qualifications from PhD to Class 10th
    public class EducationalDetailsController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Load from database if needed
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(EducationalDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to database (parse AdditionalQualificationsJSON)
                TempData["SuccessMessage"] = "Educational details saved successfully!";
                return RedirectToAction("Index", "WorkExperience");
            }

            return View(model);
        }
    }
}
