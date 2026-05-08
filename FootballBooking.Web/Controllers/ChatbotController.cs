using FootballBooking.Models;
using FootballBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FootballBooking.Web.Controllers;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly JsonKnowledgeChatbotService _chatbotService;

    public ChatbotController(JsonKnowledgeChatbotService chatbotService)
    {
        _chatbotService = chatbotService;
    }

    [HttpPost("ask")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Ask([FromBody] ChatbotAskRequest request, CancellationToken cancellationToken)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Question))
        {
            return BadRequest(new { success = false, message = "Câu hỏi không hợp lệ." });
        }

        var currentUser = GetCurrentUser();
        if (currentUser == null)
        {
            return Unauthorized(new { success = false, message = "Vui lòng đăng nhập trước khi sử dụng chatbot." });
        }

        var answer = await _chatbotService.AskAsync(request.Question, cancellationToken);
        return Ok(new
        {
            success = true,
            answer = answer.Answer,
            matchedIntent = answer.MatchedIntentId,
            matchedQuestion = answer.MatchedQuestion,
            suggestions = answer.Suggestions
        });
    }

    private User? GetCurrentUser()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (string.IsNullOrWhiteSpace(userJson))
        {
            return null;
        }

        return JsonSerializer.Deserialize<User>(userJson);
    }
}

public class ChatbotAskRequest
{
    public string Question { get; set; } = string.Empty;
}
