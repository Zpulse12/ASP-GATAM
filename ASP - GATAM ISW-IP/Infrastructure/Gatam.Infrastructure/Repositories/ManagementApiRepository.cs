using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Gatam.Application.CQRS;
using Gatam.Application.Extensions.EnvironmentHelper;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Auth0.ManagementApi.Models;

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
                Id = userId,
                Email = user.GetProperty("email").GetString(),
                Nickname = user.TryGetProperty("nickname", out var name) ? name.GetString() : string.Empty,
                Picture = user.TryGetProperty("picture", out var picture) ? picture.GetString() : null,
                IsActive = !user.TryGetProperty("blocked", out var blocked) || !blocked.GetBoolean(),
                RolesIds = new List<string>()
            };

            userDto.RolesIds = (await GetRolesByUserId(userId)).ToList();
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<UserDTO> GetUserByIdAsync(string userId)
    {
        //var users = await GetAllUsersAsync();
        var _response = await _httpClient.GetFromJsonAsync<JsonElement>($"users/{userId}");
        UserDTO _user = new UserDTO
        {
            Id = userId,
            Email = _response.GetProperty("email").GetString(),
            Nickname = _response.TryGetProperty("nickname", out var name) ? name.GetString() : string.Empty,
            Picture = _response.TryGetProperty("picture", out var picture) ? picture.GetString() : null,
            IsActive = !_response.TryGetProperty("blocked", out var blocked) || !blocked.GetBoolean(),
            RolesIds = new List<string>()
        };
        return _user; 
    }
    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
    {
        var payload = new
        {
            email = user.Email,
            username = user.UserName,
            password = user.PasswordHash, 
            connection = "Username-Password-Authentication" 
        };
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v2/users", payload);

            if (response.IsSuccessStatusCode)
            {
                var createdUser = await response.Content.ReadFromJsonAsync<ApplicationUser>();

                if (createdUser == null)
                {
                    Console.WriteLine("Failed to parse created user.");
                    return null;
                }

                Console.WriteLine("User created successfully.");


                return createdUser;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error creating user: {response.StatusCode} - {errorDetails}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the user: {ex.Message}");
            return null;
        }
    }

    public Task<bool> DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO> UpdateUserAsync(string userId, UserDTO user)
    {
        
        var response = await _httpClient.PostAsJsonAsync($"users/{userId}", user);

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

    public async Task<UserDTO> UpdateUserRoleAsync(UserDTO user)
    {
       
        var payload = new
        {
            roles = user.RolesIds.ToArray()
        };

        
        Debug.WriteLine(payload);
        string json = JsonSerializer.Serialize(payload);
        Debug.WriteLine($"Payload sent to Auth0: {json}");
        
        var response = await _httpClient.PostAsJsonAsync($"/api/v2/users/{user.Id}/roles", payload);

        if (response.IsSuccessStatusCode)
        {
            Debug.WriteLine("Roles updated successfully in Auth0.");
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
            
            var response = await _httpClient.GetFromJsonAsync<JsonElement>($"/api/v2/users/{userId}/roles");

            
            if (response.ValueKind == JsonValueKind.Array)
            {
                return response.EnumerateArray()
                               .Select(role => role.GetProperty("name").GetString())
                               .Where(name => !string.IsNullOrEmpty(name))
                               .Select(name => RoleMapper.Roles.FirstOrDefault(r => r.Key == name).Key)
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