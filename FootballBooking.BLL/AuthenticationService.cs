using FootballBooking.DAL;
using FootballBooking.Models;

namespace FootballBooking.BLL;

public class AuthenticationService
{
    private readonly UserRepository _userRepository;

    public AuthenticationService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user is null || user.Password != password)
        {
            return null;
        }

        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<int> RegisterUserAsync(string name, string email, string phone, string password)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser is not null)
        {
            throw new InvalidOperationException("Email đã được sử dụng.");
        }

        var user = new User
        {
            Name = name,
            Email = email,
            Phone = phone,
            Password = password,
            Role = UserRole.User
        };

        return await _userRepository.CreateUserAsync(user);
    }
}