using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class UserManagementService
{
    private readonly UserRepository _userRepository;

    public UserManagementService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task CreateUserAsync(User user)
    {
        await _userRepository.CreateUserAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteUserAsync(id);
    }

    public async Task UpdateUserRoleAsync(int userId, UserRole role)
    {
        await _userRepository.UpdateUserRoleAsync(userId, role);
    }

    public async Task UpdateUserPasswordAsync(int userId, string password)
    {
        await _userRepository.UpdateUserPasswordAsync(userId, password);
    }
}