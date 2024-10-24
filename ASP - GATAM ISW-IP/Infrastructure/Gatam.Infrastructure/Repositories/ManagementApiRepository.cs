using System.Net.Http.Json;
using System.Text.Json;
using Gatam.Application.CQRS;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Gatam.Infrastructure.Repositories;

public class ManagementApiRepository: IManagementApi
{
    private readonly HttpClient _httpClient;
    private readonly EnvironmentWrapper _environmentWrapper;
    public ManagementApiRepository(HttpClient httpClient, EnvironmentWrapper environmentWrapper)
    {
        _httpClient = httpClient;
        _environmentWrapper = environmentWrapper;
        _httpClient.BaseAddress = new Uri(_environmentWrapper.BASEURI);
        _httpClient.DefaultRequestHeaders.Add("Authorization", @$"Bearer {_environmentWrapper.TOKEN}");
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<JsonElement>("users");
        var userDtos = new List<UserDTO>();

        foreach (var user in response.EnumerateArray())
        {
            var userDto = new UserDTO
            {
                Id = user.GetProperty("user_id").GetString(),
                Email = user.GetProperty("email").GetString(),
                Username = user.TryGetProperty("nickname", out var name) ? name.GetString() : string.Empty,
                Picture = user.TryGetProperty("picture", out var picture) ? picture.GetString() : null,
                IsActive = !user.TryGetProperty("blocked", out var blocked) || !blocked.GetBoolean()

            };

            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public Task<bool> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> UpdateUserAsync(string userId, UserDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO> UpdateUserStatusAsync(string userId, bool isActive)
    {
        var payload = new 
        {
            blocked = !isActive
        };
        
        var response = await _httpClient.PutAsJsonAsync($"users/{userId}", payload);

        if (!response.IsSuccessStatusCode)
        {
            var errorDetails = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Error updating user status: {response.StatusCode} - {errorDetails}");
            return null;
        }

        var updatedUser = await response.Content.ReadFromJsonAsync<UserDTO>();
        return updatedUser;
    }

    public async Task<UserDTO> UpdateUserRoleAsync(UserDTO user, List<string> roles)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"users/{user.Id}/roles", new { roles = roles });
        if (response.IsSuccessStatusCode)
        {
            return user;
        }
        return null;
    }

    public async Task<IEnumerable<string>> GetRolesByUserId(string userId)
    {
        JsonElement response = await _httpClient.GetFromJsonAsync<JsonElement>($"/users/{userId}/roles");
        throw new NotImplementedException();
    }
}