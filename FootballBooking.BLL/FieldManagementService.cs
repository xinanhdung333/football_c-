using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class FieldManagementService
{
    private readonly FieldRepository _fieldRepository;

    public FieldManagementService(FieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public async Task<IEnumerable<Field>> GetFieldsAsync()
    {
        return await _fieldRepository.GetAllFieldsAsync();
    }

    public async Task<Field?> GetFieldByIdAsync(int id)
    {
        return await _fieldRepository.GetFieldByIdAsync(id);
    }

    public async Task CreateFieldAsync(Field field)
    {
        ValidateField(field);
        await _fieldRepository.CreateFieldAsync(field);
    }

    public async Task UpdateFieldPriceAsync(int id, decimal pricePerHour)
    {
        if (pricePerHour <= 0)
        {
            throw new ArgumentException("Giá sân phải lớn hơn 0.", nameof(pricePerHour));
        }

        await _fieldRepository.UpdateFieldPriceAsync(id, pricePerHour);
    }

    public async Task UpdateFieldPeakPricingAsync(int id, TimeSpan? peakStartTime, TimeSpan? peakEndTime, decimal? peakPricePerHour)
    {
        ValidatePeakPricing(peakStartTime, peakEndTime, peakPricePerHour);
        await _fieldRepository.UpdateFieldPeakPricingAsync(id, peakStartTime, peakEndTime, peakPricePerHour);
    }

    public async Task UpdateFieldAsync(Field field)
    {
        ValidateField(field);
        await _fieldRepository.UpdateFieldAsync(field);
    }

    public async Task DeleteFieldAsync(int id)
    {
        await _fieldRepository.DeleteFieldAsync(id);
    }

    private static void ValidateField(Field field)
    {
        if (field.PricePerHour <= 0)
        {
            throw new ArgumentException("Giá sân phải lớn hơn 0.", nameof(field));
        }

        ValidatePeakPricing(field.PeakStartTime, field.PeakEndTime, field.PeakPricePerHour);
    }

    private static void ValidatePeakPricing(TimeSpan? peakStartTime, TimeSpan? peakEndTime, decimal? peakPricePerHour)
    {
        var hasAnyPeakValue = peakStartTime.HasValue || peakEndTime.HasValue || peakPricePerHour.HasValue;
        if (!hasAnyPeakValue)
        {
            return;
        }

        if (!peakStartTime.HasValue || !peakEndTime.HasValue || !peakPricePerHour.HasValue)
        {
            throw new ArgumentException("Vui lòng nhập đủ giờ bắt đầu, giờ kết thúc và giá tăng.");
        }

        if (peakEndTime.Value <= peakStartTime.Value)
        {
            throw new ArgumentException("Giờ kết thúc tăng giá phải lớn hơn giờ bắt đầu.");
        }

        if (peakPricePerHour.Value <= 0)
        {
            throw new ArgumentException("Giá tăng theo khung giờ phải lớn hơn 0.");
        }
    }
}
