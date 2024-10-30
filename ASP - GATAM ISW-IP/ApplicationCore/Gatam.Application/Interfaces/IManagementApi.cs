using Gatam.Application.CQRS;

namespace Gatam.Application.Interfaces;

public interface IManagementApi
{
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(string userId);
    Task<UserDTO> UpdateUserAsync(string userId, UserDTO user);
    Task<UserDTO> UpdateUserStatusAsync(string userId, bool isActive);
    Task<UserDTO> UpdateUserRoleAsync(UserDTO user, IEnumerable<string> roles);
    Task<IEnumerable<string>> GetRolesByUserId(string userId);
    Task<UserDTO> GetUserByIdAsync(string userId);
}