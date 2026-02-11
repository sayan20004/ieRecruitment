using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    // Handles address and personal details
    public class PersonalDetailsController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Load from database if needed
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(PersonalDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Copy current address to permanent address if checkbox is checked
                if (model.SameAsCurrent)
                {
                    model.PermanentAddressLine1 = model.CurrentAddressLine1;
                    model.PermanentAddressLine2 = model.CurrentAddressLine2;
                    model.PermanentCity = model.CurrentCity;
                    model.PermanentPostOffice = model.CurrentPostOffice;
                    model.PermanentPinCode = model.CurrentPinCode;
                    model.PermanentDistrict = model.CurrentDistrict;
                    model.PermanentState = model.CurrentState;
                    model.PermanentCountry = model.CurrentCountry;
                }

                // TODO: Save to database
                TempData["SuccessMessage"] = "Personal details saved successfully!";
                return RedirectToAction("Index", "FamilyDetails");
            }

            return View(model);
        }
    }
}
