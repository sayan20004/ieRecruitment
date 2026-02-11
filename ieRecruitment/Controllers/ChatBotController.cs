using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Services;

namespace ieRecruitment.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly IGeminiChatbotService _chatbotService;

        public ChatBotController(IGeminiChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        /// <summary>
        /// AJAX endpoint: accepts a user message + conversation history,
        /// returns AI response from Gemini.
        /// </summary>
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
                return Json(new { success = false, reply = "Please type a message." });

            // Build conversation history (limit to last 20 messages to stay within token limits)
            var history = new List<ChatMessage>();

            if (request.History != null)
            {
                var recent = request.History.Count > 20
                    ? request.History.Skip(request.History.Count - 20).ToList()
                    : request.History;

                foreach (var h in recent)
                {
                    history.Add(new ChatMessage
                    {
                        Role = h.Role == "user" ? "user" : "model",
                        Text = h.Text ?? ""
                    });
                }
            }

            // Add the current message
            history.Add(new ChatMessage { Role = "user", Text = request.Message });

            // Build dynamic page context
            var pageContext = BuildPageContext(request);

            var reply = await _chatbotService.GetResponseAsync(history, pageContext);

            return Json(new { success = true, reply });
        }

        private string BuildPageContext(ChatRequest request)
        {
            var sb = new System.Text.StringBuilder();

            if (!string.IsNullOrWhiteSpace(request.CurrentPage))
                sb.AppendLine($"The candidate is currently on: {request.CurrentPage}.");

            if (request.AutoFilledFields != null && request.AutoFilledFields.Count > 0)
                sb.AppendLine($"Fields auto-filled from their CV on this page: {string.Join(", ", request.AutoFilledFields)}. Remind them to review these.");
            else
                sb.AppendLine("No fields were auto-filled from CV on this page.");

            if (request.PageFields != null && request.PageFields.Count > 0)
                sb.AppendLine($"Visible fields on this page: {string.Join(", ", request.PageFields)}.");

            return sb.ToString();
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public List<ChatHistoryItem>? History { get; set; }
        public string? CurrentPage { get; set; }
        public List<string>? AutoFilledFields { get; set; }
        public List<string>? PageFields { get; set; }
    }

    public class ChatHistoryItem
    {
        public string Role { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
