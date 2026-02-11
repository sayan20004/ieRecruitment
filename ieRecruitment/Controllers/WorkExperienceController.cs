using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    // Handles work experience: Fresher, Experienced, or Sales
    public class WorkExperienceController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Load from database if needed
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(WorkExperienceViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to database (parse JSON based on experience type)
                TempData["SuccessMessage"] = "Work experience saved successfully!";
                return RedirectToAction("Index", "OtherDetails");
            }

            return View(model);
        }
    }
}
