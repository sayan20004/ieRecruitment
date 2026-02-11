using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index(string? appId)
        {
            var model = BuildSampleDashboard();

            // Set the active application (default to first)
            model.ActiveApplicationId = !string.IsNullOrEmpty(appId) && model.Applications.Any(a => a.Id == appId)
                ? appId
                : model.Applications.FirstOrDefault()?.Id ?? "";

            return View(model);
        }

        // ── Reschedule interview (AJAX) ──
        [HttpPost]
        public IActionResult RescheduleInterview(string interviewId, string newDate, string newTime)
        {
            // TODO: Persist to database
            return Json(new { success = true, message = "Interview rescheduled successfully." });
        }

        // ── Sample data for demo ──
        private static DashboardViewModel BuildSampleDashboard()
        {
            var now = DateTime.Now;

            var app1 = new ApplicationEntry
            {
                Id = "APP-001",
                CandidateCode = "MEN-2026-008735",
                Position = "Business Development Manager (BDM) - Kolkata",
                Department = "Sales & Marketing",
                ApplicationStatus = "Interview Scheduled",
                AppliedDate = now.AddDays(-5),
                LastUpdated = now.AddHours(-6),
                FormCompletionPercent = 100,
                TotalDocumentsUploaded = 3,
                FormSectionsCompleted = 7,
                TotalFormSections = 7,
                HRContactName = "Priya Sharma",
                HRContactEmail = "priya.sharma@mendine.com",
                HRContactPhone = "+91 98765 12345",

                Timeline = new List<TimelineEvent>
                {
                    new() { Title = "Application Submitted", Description = "All 7 sections completed and submitted.", Date = now.AddDays(-5), Icon = "bi-file-earmark-check", Status = "completed" },
                    new() { Title = "Application Received", Description = "Your application has been received by the HR team.", Date = now.AddDays(-5), Icon = "bi-inbox", Status = "completed" },
                    new() { Title = "Under Review", Description = "HR is reviewing your profile and documents.", Date = now.AddDays(-3), Icon = "bi-search", Status = "completed" },
                    new() { Title = "Shortlisted", Description = "Congratulations! You've been shortlisted for the interview.", Date = now.AddDays(-1), Icon = "bi-star", Status = "completed" },
                    new() { Title = "Interview Scheduled", Description = "Round 1 – HR Screening scheduled.", Date = now, Icon = "bi-camera-video", Status = "active" },
                    new() { Title = "Final Decision", Description = "Pending interview completion.", Date = now.AddDays(7), Icon = "bi-trophy", Status = "pending" },
                },

                UpcomingInterview = new InterviewInfo
                {
                    Id = "INT-001",
                    Round = "Round 1 – HR Screening",
                    Type = "Video Call",
                    ScheduledDate = now.AddDays(2),
                    Time = "10:00 AM – 10:45 AM",
                    InterviewerName = "Priya Sharma",
                    InterviewerRole = "Senior HR Manager",
                    MeetingLink = "https://meet.google.com/abc-defg-hij",
                    Status = "Scheduled",
                    CanReschedule = true
                },

                MockInterview = new MockInterviewInfo
                {
                    IsAvailable = true,
                    Title = "AI Mock Interview",
                    Description = "Practice common HR and BDM interview questions with our AI interviewer.",
                    QuestionsCount = 15,
                    DurationMinutes = 30,
                    AttemptsUsed = 0,
                    MaxAttempts = 3
                },

                Documents = new List<DocumentInfo>
                {
                    new() { Id = "DOC-001", Name = "Resume_SayanMaity.pdf", Type = "Resume", UploadedDate = now.AddDays(-5), FileSize = "245 KB", Status = "Verified", DownloadUrl = "/documents/resume.pdf" },
                    new() { Id = "DOC-002", Name = "CoverLetter_BDM.pdf", Type = "Cover Letter", UploadedDate = now.AddDays(-5), FileSize = "128 KB", Status = "Verified", DownloadUrl = "/documents/cover.pdf" },
                    new() { Id = "DOC-003", Name = "Certificates.pdf", Type = "Certificates", UploadedDate = now.AddDays(-4), FileSize = "1.2 MB", Status = "Verified", DownloadUrl = "/documents/certs.pdf" }
                },

                PastInterviews = new List<InterviewInfo>()
            };

            var app2 = new ApplicationEntry
            {
                Id = "APP-002",
                CandidateCode = "MEN-2026-008736",
                Position = "Senior Software Engineer - Bangalore",
                Department = "Engineering",
                ApplicationStatus = "Under Review",
                AppliedDate = now.AddDays(-2),
                LastUpdated = now.AddDays(-1),
                FormCompletionPercent = 100,
                TotalDocumentsUploaded = 2,
                FormSectionsCompleted = 7,
                TotalFormSections = 7,
                HRContactName = "Rahul Verma",
                HRContactEmail = "rahul.verma@mendine.com",
                HRContactPhone = "+91 98765 54321",

                Timeline = new List<TimelineEvent>
                {
                    new() { Title = "Application Submitted", Description = "All 7 sections completed and submitted.", Date = now.AddDays(-2), Icon = "bi-file-earmark-check", Status = "completed" },
                    new() { Title = "Application Received", Description = "Your application has been received by the HR team.", Date = now.AddDays(-2), Icon = "bi-inbox", Status = "completed" },
                    new() { Title = "Under Review", Description = "HR is reviewing your profile and documents.", Date = now.AddDays(-1), Icon = "bi-search", Status = "active" },
                    new() { Title = "Shortlisted", Description = "Awaiting review completion.", Date = now.AddDays(3), Icon = "bi-star", Status = "pending" },
                    new() { Title = "Interview", Description = "Pending shortlist.", Date = now.AddDays(7), Icon = "bi-camera-video", Status = "pending" },
                    new() { Title = "Final Decision", Description = "Pending.", Date = now.AddDays(14), Icon = "bi-trophy", Status = "pending" },
                },

                MockInterview = new MockInterviewInfo
                {
                    IsAvailable = true,
                    Title = "AI Mock Interview",
                    Description = "Practice engineering interview questions with our AI interviewer.",
                    QuestionsCount = 20,
                    DurationMinutes = 45,
                    AttemptsUsed = 1,
                    MaxAttempts = 3,
                    LastScore = 72
                },

                Documents = new List<DocumentInfo>
                {
                    new() { Id = "DOC-004", Name = "Resume_SayanMaity_Tech.pdf", Type = "Resume", UploadedDate = now.AddDays(-2), FileSize = "312 KB", Status = "Verified", DownloadUrl = "/documents/resume_tech.pdf" },
                    new() { Id = "DOC-005", Name = "GitHub_Portfolio.pdf", Type = "Portfolio", UploadedDate = now.AddDays(-2), FileSize = "890 KB", Status = "Pending", DownloadUrl = "/documents/portfolio.pdf" }
                },

                PastInterviews = new List<InterviewInfo>()
            };

            var app3 = new ApplicationEntry
            {
                Id = "APP-003",
                CandidateCode = "MEN-2026-008737",
                Position = "Product Manager - Mumbai",
                Department = "Product",
                ApplicationStatus = "Applied",
                AppliedDate = now,
                LastUpdated = now,
                FormCompletionPercent = 85,
                TotalDocumentsUploaded = 1,
                FormSectionsCompleted = 6,
                TotalFormSections = 7,
                HRContactName = "Anjali Desai",
                HRContactEmail = "anjali.desai@mendine.com",
                HRContactPhone = "+91 98765 67890",

                Timeline = new List<TimelineEvent>
                {
                    new() { Title = "Application Submitted", Description = "6 of 7 sections completed.", Date = now, Icon = "bi-file-earmark-check", Status = "active" },
                    new() { Title = "Application Received", Description = "Pending completion.", Date = now.AddDays(1), Icon = "bi-inbox", Status = "pending" },
                    new() { Title = "Under Review", Description = "Pending.", Date = now.AddDays(5), Icon = "bi-search", Status = "pending" },
                },

                Documents = new List<DocumentInfo>
                {
                    new() { Id = "DOC-006", Name = "Resume_SayanMaity_PM.pdf", Type = "Resume", UploadedDate = now, FileSize = "198 KB", Status = "Pending", DownloadUrl = "/documents/resume_pm.pdf" }
                }
            };

            return new DashboardViewModel
            {
                FullName = "Sayan Maity",
                Email = "sayan.maity@email.com",
                Phone = "+91 98765 43210",

                Applications = new List<ApplicationEntry> { app1, app2, app3 },
                ActiveApplicationId = app1.Id,

                Notifications = new List<NotificationItem>
                {
                    new() { Message = "Your interview for Round 1 – HR Screening (BDM) has been scheduled.", Date = now.AddHours(-6), Type = "success", IsRead = false },
                    new() { Message = "Congratulations! You have been shortlisted for the BDM position.", Date = now.AddDays(-1), Type = "success", IsRead = true },
                    new() { Message = "Application for Senior Software Engineer is now under review.", Date = now.AddDays(-1), Type = "info", IsRead = false },
                    new() { Message = "Application for Product Manager submitted. Complete remaining sections.", Date = now, Type = "warning", IsRead = false },
                    new() { Message = "Application submitted successfully for BDM. Code: MEN-2026-008735", Date = now.AddDays(-5), Type = "info", IsRead = true },
                }
            };
        }
    }
}
