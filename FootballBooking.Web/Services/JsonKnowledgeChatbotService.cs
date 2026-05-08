using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FootballBooking.Web.Services;

public class JsonKnowledgeChatbotService
{
    private static readonly Regex MultiWhitespace = new(@"\s+", RegexOptions.Compiled);
    private readonly string _knowledgeFilePath;
    private readonly SemaphoreSlim _cacheLock = new(1, 1);
    private ChatbotKnowledgeDocument? _cachedDocument;
    private DateTime _lastLoadedWriteTimeUtc;

    public JsonKnowledgeChatbotService(IWebHostEnvironment environment)
    {
        _knowledgeFilePath = Path.Combine(environment.ContentRootPath, "App_Data", "chatbot_knowledge.json");
    }

    public async Task<ChatbotAnswer> AskAsync(string question, CancellationToken cancellationToken = default)
    {
        var normalizedQuestion = Normalize(question);
        if (string.IsNullOrWhiteSpace(normalizedQuestion))
        {
            return new ChatbotAnswer
            {
                Answer = "Bạn hãy nhập câu hỏi cụ thể hơn để mình hỗ trợ.",
                Suggestions = Array.Empty<string>()
            };
        }

        var knowledge = await LoadKnowledgeAsync(cancellationToken);
        if (knowledge.Entries.Count == 0)
        {
            return new ChatbotAnswer
            {
                Answer = "Nguồn dữ liệu chatbot đang trống. Vui lòng cấu hình file JSON kiến thức.",
                Suggestions = Array.Empty<string>()
            };
        }

        var rankedEntry = knowledge.Entries
            .Select(entry => new { Entry = entry, Score = ScoreEntry(entry, normalizedQuestion) })
            .OrderByDescending(item => item.Score)
            .FirstOrDefault();

        if (rankedEntry == null || rankedEntry.Score <= 0)
        {
            return new ChatbotAnswer
            {
                Answer = knowledge.DefaultAnswer,
                Suggestions = knowledge.Entries
                    .Select(item => item.Question)
                    .Where(item => !string.IsNullOrWhiteSpace(item))
                    .Take(3)
                    .ToArray()
            };
        }

        return new ChatbotAnswer
        {
            Answer = rankedEntry.Entry.Answer,
            MatchedIntentId = rankedEntry.Entry.Id,
            MatchedQuestion = rankedEntry.Entry.Question,
            Suggestions = rankedEntry.Entry.Suggestions.ToArray()
        };
    }

    private async Task<ChatbotKnowledgeDocument> LoadKnowledgeAsync(CancellationToken cancellationToken)
    {
        var fileInfo = new FileInfo(_knowledgeFilePath);
        if (!fileInfo.Exists)
        {
            return new ChatbotKnowledgeDocument();
        }

        var writeTimeUtc = fileInfo.LastWriteTimeUtc;
        if (_cachedDocument != null && writeTimeUtc <= _lastLoadedWriteTimeUtc)
        {
            return _cachedDocument;
        }

        await _cacheLock.WaitAsync(cancellationToken);
        try
        {
            fileInfo.Refresh();
            writeTimeUtc = fileInfo.LastWriteTimeUtc;
            if (_cachedDocument != null && writeTimeUtc <= _lastLoadedWriteTimeUtc)
            {
                return _cachedDocument;
            }

            await using var stream = File.OpenRead(_knowledgeFilePath);
            var rawDocument = await JsonSerializer.DeserializeAsync<ChatbotKnowledgeJson>(
                stream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                cancellationToken);

            var mapped = new ChatbotKnowledgeDocument
            {
                DefaultAnswer = string.IsNullOrWhiteSpace(rawDocument?.DefaultAnswer)
                    ? "Mình chưa có thông tin cho câu hỏi này."
                    : rawDocument!.DefaultAnswer.Trim(),
                Entries = rawDocument?.Entries?
                    .Where(entry =>
                        !string.IsNullOrWhiteSpace(entry.Question) &&
                        !string.IsNullOrWhiteSpace(entry.Answer) &&
                        entry.Keywords?.Any() == true)
                    .Select(entry => new ChatbotKnowledgeEntry
                    {
                        Id = entry.Id?.Trim(),
                        Question = entry.Question!.Trim(),
                        Answer = entry.Answer!.Trim(),
                        Keywords = entry.Keywords!.Select(keyword => Normalize(keyword)).Where(keyword => keyword.Length > 0).Distinct().ToArray(),
                        Suggestions = entry.Suggestions?.Where(suggestion => !string.IsNullOrWhiteSpace(suggestion)).Select(suggestion => suggestion.Trim()).Distinct().Take(5).ToArray()
                                      ?? Array.Empty<string>()
                    })
                    .ToList() ?? new List<ChatbotKnowledgeEntry>()
            };

            _cachedDocument = mapped;
            _lastLoadedWriteTimeUtc = writeTimeUtc;
            return mapped;
        }
        finally
        {
            _cacheLock.Release();
        }
    }

    private static int ScoreEntry(ChatbotKnowledgeEntry entry, string normalizedQuestion)
    {
        var score = 0;
        foreach (var keyword in entry.Keywords)
        {
            if (normalizedQuestion.Contains(keyword, StringComparison.Ordinal))
            {
                score += Math.Max(2, keyword.Length / 4);
            }
        }

        var normalizedIntentQuestion = Normalize(entry.Question);
        if (normalizedQuestion.Contains(normalizedIntentQuestion, StringComparison.Ordinal))
        {
            score += 6;
        }

        return score;
    }

    private static string Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var lowered = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
        var chars = lowered.Where(character => CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark).ToArray();
        var noAccent = new string(chars).Normalize(NormalizationForm.FormC);
        noAccent = noAccent.Replace('đ', 'd');

        var sanitized = new string(noAccent.Select(character => char.IsLetterOrDigit(character) ? character : ' ').ToArray());
        return MultiWhitespace.Replace(sanitized, " ").Trim();
    }

    private sealed class ChatbotKnowledgeJson
    {
        public string? DefaultAnswer { get; set; }
        public List<ChatbotKnowledgeEntryJson>? Entries { get; set; }
    }

    private sealed class ChatbotKnowledgeEntryJson
    {
        public string? Id { get; set; }
        public string? Question { get; set; }
        public List<string>? Keywords { get; set; }
        public string? Answer { get; set; }
        public List<string>? Suggestions { get; set; }
    }
}

public class ChatbotAnswer
{
    public string Answer { get; set; } = string.Empty;
    public string? MatchedIntentId { get; set; }
    public string? MatchedQuestion { get; set; }
    public IReadOnlyList<string> Suggestions { get; set; } = Array.Empty<string>();
}

internal class ChatbotKnowledgeDocument
{
    public string DefaultAnswer { get; set; } = "Mình chưa có thông tin cho câu hỏi này.";
    public List<ChatbotKnowledgeEntry> Entries { get; set; } = new();
}

internal class ChatbotKnowledgeEntry
{
    public string? Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string[] Keywords { get; set; } = Array.Empty<string>();
    public string Answer { get; set; } = string.Empty;
    public string[] Suggestions { get; set; } = Array.Empty<string>();
}
