using Application.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services;

public class UserService(IUserRepository userRepository)
{
    public async Task CreateUserAsync(string username)
    {
        var user = new User(username);
        await userRepository.AddAsync(user);
    }
    
    public Task UpdateUserAsync(User user)
    {
        return userRepository.UpdateAsync(user);
    }
    
    public Task DeleteUserAsync(User user)
    {
        return userRepository.DeleteAsync(user);
    }
    
    public Task<User?> GetUserByIdAsync(Guid id)
    {
        return userRepository.GetByIdAsync(id);
    }
}