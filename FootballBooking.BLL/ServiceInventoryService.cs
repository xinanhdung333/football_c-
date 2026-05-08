using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class ServiceInventoryService
{
    private readonly ServiceRepository _serviceRepository;

    public ServiceInventoryService(ServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<Service>> GetServicesAsync()
    {
        return await _serviceRepository.GetAllServicesAsync();
    }

    public async Task<Service?> GetServiceByIdAsync(int id)
    {
        return await _serviceRepository.GetServiceByIdAsync(id);
    }

    public async Task UpdateStockAsync(int serviceId, int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("Số lượng phải là số không âm.", nameof(quantity));
        }

        await _serviceRepository.UpdateQuantityAsync(serviceId, quantity);
    }

    public async Task CreateServiceAsync(Service service)
    {
        await _serviceRepository.CreateServiceAsync(service);
    }

    public async Task UpdateServiceAsync(Service service)
    {
        await _serviceRepository.UpdateServiceAsync(service);
    }

    public async Task DeleteServiceAsync(int id)
    {
        await _serviceRepository.DeleteServiceAsync(id);
    }
}