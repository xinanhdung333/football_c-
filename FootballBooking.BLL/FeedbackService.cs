using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class FeedbackService
{
    private readonly FeedbackRepository _feedbackRepository;

    public FeedbackService(FeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async Task SubmitFeedbackAsync(int userId, int? bookingId, int? serviceId, string message, byte rating)
    {
        if (rating < 1 || rating > 5)
        {
            throw new ArgumentException("Đánh giá phải từ 1 đến 5 sao.", nameof(rating));
        }

        var feedback = new Feedback
        {
            UserId = userId,
            BookingId = bookingId,
            ServiceId = serviceId,
            Message = message,
            Rating = rating
        };

        await _feedbackRepository.CreateFeedbackAsync(feedback);
    }

    public async Task<IEnumerable<Feedback>> GetFeedbacksForBookingAsync(int bookingId)
    {
        return await _feedbackRepository.GetFeedbacksByBookingIdAsync(bookingId);
    }

    public async Task<IEnumerable<FeedbackWithField>> GetFeedbacksByUserIdAsync(int userId)
    {
        return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
    }

    public async Task<IEnumerable<FeedbackWithDetails>> GetAllFeedbacksWithDetailsAsync()
    {
        return await _feedbackRepository.GetAllFeedbacksWithDetailsAsync();
    }

    public async Task DeleteFeedbackAsync(int id)
    {
        await _feedbackRepository.DeleteFeedbackAsync(id);
    }

    public async Task ReplyFeedbackAsync(int feedbackId, int adminUserId, string replyMessage)
    {
        if (feedbackId <= 0)
        {
            throw new ArgumentException("Feedback không hợp lệ.", nameof(feedbackId));
        }

        if (string.IsNullOrWhiteSpace(replyMessage))
        {
            throw new ArgumentException("Nội dung phản hồi không được để trống.", nameof(replyMessage));
        }

        var normalizedReply = replyMessage.Trim();
        if (normalizedReply.Length > 1000)
        {
            throw new ArgumentException("Nội dung phản hồi không được vượt quá 1000 ký tự.", nameof(replyMessage));
        }

        await _feedbackRepository.ReplyFeedbackAsync(feedbackId, adminUserId, normalizedReply);
    }
}
