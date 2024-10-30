using System.Net.Http.Json;
using System.Text.Json;
using Gatam.Application.CQRS;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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

            var userId = user.GetProperty("user_id").GetString();
         

            var userDto = new UserDTO
            {
                Id = user.GetProperty("user_id").GetString(),
                Email = user.GetProperty("email").GetString(),
                Username = user.TryGetProperty("nickname", out var name) ? name.GetString() : string.Empty,
                Picture = user.TryGetProperty("picture", out var picture) ? picture.GetString() : null,
                IsActive = !user.TryGetProperty("blocked", out var blocked) || !blocked.GetBoolean(),
                RolesIds = await GetRolesByUserId(userId)
            };

            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<UserDTO> GetUserByIdAsync(string userId)
    {
        var users = await GetAllUsersAsync(); // Haal alle gebruikers op
        return users.FirstOrDefault(u => u.Id == userId); // Zoek naar de gebruiker met de opgegeven ID
    }


    public Task<bool> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO> UpdateUserAsync(string userId, UserDTO user)
    {
        if (userId != user.Id)
        {
            throw new ArgumentException("User ID mismatch.");
        }

        var response = await _httpClient.PutAsJsonAsync($"users/{userId}", user);

        if (response.IsSuccessStatusCode)
        {
            var updatedUser = await response.Content.ReadFromJsonAsync<UserDTO>();
            if (updatedUser == null)
            {
                Console.WriteLine("Fout bij het parsen van de gebruiker na update.");
            }
            return updatedUser;
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error updating user: {response.StatusCode} - {errorDetails}");
        return user;
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

    public async Task<UserDTO> UpdateUserRoleAsync(UserDTO user, IEnumerable<string> roles)
    {
        var roleIds = roles.Select(role => RoleMapper.GetRoleId(role)).Where(id => id != null).ToArray();
        var payload = new
        {
            roles = roleIds
        };
        var response = await _httpClient.PostAsJsonAsync($"users/{user.Id}/roles", payload);

        if (response.IsSuccessStatusCode)
        {
            return user;
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error updating user roles: {response.StatusCode} - {errorDetails}");
        return null;
    }

    public async Task<IEnumerable<string>> GetRolesByUserId(string userId)
    {
        try
        {
            // Auth0 API-aanroep om rollen voor de gebruiker op te halen
            JsonElement response = await _httpClient.GetFromJsonAsync<JsonElement>($"users/{userId}/roles");

            // Controleer of de response een array van rollen bevat
            if (response.ValueKind == JsonValueKind.Array)
            {
                return response.EnumerateArray()
                               .Select(role => role.GetProperty("name").GetString())
                               .Where(name => !string.IsNullOrEmpty(name))
                               .ToList();
            }

            return new List<string>(); 
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching roles for user {userId}: {ex.Message}");
            return new List<string>(); 
        }
    }

   
}