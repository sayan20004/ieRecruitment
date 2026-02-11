using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;
using ieRecruitment.Services;

namespace ieRecruitment.Controllers
{
    // Entry point - handles candidate basic info and file uploads
    public class RecruitmentFormController : Controller
    {
        private readonly IGeminiCvService _geminiCvService;
        private readonly ILogger<RecruitmentFormController> _logger;

        public RecruitmentFormController(IGeminiCvService geminiCvService, ILogger<RecruitmentFormController> logger)
        {
            _geminiCvService = geminiCvService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new RecruitmentFormViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RecruitmentFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save files and data to database
                TempData["SuccessMessage"] = "Recruitment form submitted successfully!";
                return RedirectToAction("Index", "ProfessionalSummary");
            }

            return View(model);
        }

        /// <summary>
        /// AJAX endpoint: accepts a CV file, sends it to Google Gemini for extraction,
        /// and returns structured JSON data to auto-fill recruitment form fields.
        /// </summary>
        [HttpPost]
        [RequestSizeLimit(10_485_760)] // 10 MB
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ExtractCv(IFormFile cvFile)
        {
            if (cvFile == null || cvFile.Length == 0)
                return Json(new { success = false, message = "No file uploaded." });

            try
            {
                _logger.LogInformation("CV extraction requested: {FileName} ({Size} bytes)", cvFile.FileName, cvFile.Length);
                var data = await _geminiCvService.ExtractFromCvAsync(cvFile);

                if (data == null)
                    return Json(new { success = false, message = "Could not extract data from the CV. Please try a different file." });

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CV extraction failed for {FileName}", cvFile.FileName);
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
