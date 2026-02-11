namespace ieRecruitment.Models
{
    public class DashboardViewModel
    {
        // ── Candidate Info (shared across applications) ──
        public string FullName { get; set; } = "Sayan Maity";
        public string Email { get; set; } = "sayan.maity@email.com";
        public string Phone { get; set; } = "+91 98765 43210";
        public string PhotoUrl { get; set; } = "/img/default-avatar.png";

        // ── Multiple Applications ──
        public List<ApplicationEntry> Applications { get; set; } = new();
        public string ActiveApplicationId { get; set; } = "";

        // Currently selected application (computed)
        public ApplicationEntry Active => Applications.FirstOrDefault(a => a.Id == ActiveApplicationId) ?? Applications.FirstOrDefault() ?? new();

        // ── Notifications (global) ──
        public List<NotificationItem> Notifications { get; set; } = new();
    }

    // ── One Application per Position ──
    public class ApplicationEntry
    {
        public string Id { get; set; } = "";
        public string CandidateCode { get; set; } = "";
        public string Position { get; set; } = "";
        public string Department { get; set; } = "";
        public string ApplicationStatus { get; set; } = "Under Review";
        public DateTime AppliedDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int FormCompletionPercent { get; set; } = 100;
        public int TotalDocumentsUploaded { get; set; } = 2;
        public int FormSectionsCompleted { get; set; } = 7;
        public int TotalFormSections { get; set; } = 7;

        // Per-application data
        public List<TimelineEvent> Timeline { get; set; } = new();
        public InterviewInfo? UpcomingInterview { get; set; }
        public List<InterviewInfo> PastInterviews { get; set; } = new();
        public MockInterviewInfo? MockInterview { get; set; }
        public List<DocumentInfo> Documents { get; set; } = new();
        public string? HRContactName { get; set; }
        public string? HRContactEmail { get; set; }
        public string? HRContactPhone { get; set; }
    }

    public class TimelineEvent
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; }
        public string Icon { get; set; } = "bi-circle-fill";
        public string Status { get; set; } = "completed"; // completed, active, pending
    }

    public class InterviewInfo
    {
        public string Id { get; set; } = "";
        public string Round { get; set; } = "";       // e.g. "Round 1 – HR Screening"
        public string Type { get; set; } = "";         // Video Call, In-Person, Phone
        public DateTime ScheduledDate { get; set; }
        public string Time { get; set; } = "";         // e.g. "10:00 AM – 10:45 AM"
        public string InterviewerName { get; set; } = "";
        public string InterviewerRole { get; set; } = "";
        public string MeetingLink { get; set; } = "";
        public string Location { get; set; } = "";
        public string Status { get; set; } = "Scheduled";  // Scheduled, Completed, Cancelled, Rescheduled
        public string? Feedback { get; set; }
        public bool CanReschedule { get; set; } = true;
    }

    public class MockInterviewInfo
    {
        public bool IsAvailable { get; set; } = true;
        public string Title { get; set; } = "AI Mock Interview";
        public string Description { get; set; } = "Practice with our AI interviewer to prepare for your upcoming round.";
        public int QuestionsCount { get; set; } = 15;
        public int DurationMinutes { get; set; } = 30;
        public int? LastScore { get; set; }
        public int AttemptsUsed { get; set; } = 0;
        public int MaxAttempts { get; set; } = 3;
    }

    public class NotificationItem
    {
        public string Message { get; set; } = "";
        public DateTime Date { get; set; }
        public string Type { get; set; } = "info";  // info, success, warning, danger
        public bool IsRead { get; set; } = false;
    }

    public class DocumentInfo
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";  // Resume, Cover Letter, Certificate, etc.
        public DateTime UploadedDate { get; set; }
        public string FileSize { get; set; } = "";
        public string Status { get; set; } = "Verified";  // Verified, Pending, Rejected
        public string? DownloadUrl { get; set; }
    }
}
