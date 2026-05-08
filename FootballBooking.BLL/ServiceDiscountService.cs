using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class ServiceDiscountService
{
    private readonly ServiceDiscountRepository _discountRepository;

    public ServiceDiscountService(ServiceDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public async Task<IEnumerable<ServiceDiscount>> GetDiscountsAsync()
    {
        return await _discountRepository.GetAllAsync();
    }

    public async Task<ServicePriceQuote> GetPriceQuoteAsync(Service service, DateTime? at = null)
    {
        var now = at ?? DateTime.Now;
        var discounts = await _discountRepository.GetActiveForServiceAsync(service.Id, now.TimeOfDay);
        var discount = discounts.FirstOrDefault();
        var multiplier = discount?.Multiplier ?? 1m;
        var finalPrice = Math.Round(service.Price * multiplier, 2);

        return new ServicePriceQuote(
            service.Id,
            service.Price,
            finalPrice,
            discount?.DiscountPercent ?? 0,
            discount?.Note);
    }

    public async Task<int> CreateDiscountAsync(ServiceDiscount discount)
    {
        Validate(discount);
        return await _discountRepository.CreateAsync(discount);
    }

    public async Task DeleteDiscountAsync(int id)
    {
        await _discountRepository.DeleteAsync(id);
    }

    private static void Validate(ServiceDiscount discount)
    {
        if (discount.EndTime <= discount.StartTime)
        {
            throw new ArgumentException("Gio ket thuc giam gia phai lon hon gio bat dau.", nameof(discount));
        }

        if (discount.Multiplier <= 0 || discount.Multiplier > 1)
        {
            throw new ArgumentException("Multiplier phai nam trong khoang 0-1.", nameof(discount));
        }
    }
}

public record ServicePriceQuote(
    int ServiceId,
    decimal OriginalPrice,
    decimal FinalPrice,
    decimal DiscountPercent,
    string? Note);
