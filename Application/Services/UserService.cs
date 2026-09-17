using Application.DTOs.Requests;
using Application.Exceptions.NotFound;
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
    
    public async Task UpdateUserAsync(UpdateUserRequest request)
    {
        var user = await userRepository.GetByIdAsync(request.Id);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        
        user.Update(request.Username);
        await userRepository.UpdateAsync(user);
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