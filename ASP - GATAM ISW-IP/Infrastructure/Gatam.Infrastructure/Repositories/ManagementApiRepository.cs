using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using Gatam.Application.CQRS;
using Gatam.Application.Extensions.EnvironmentHelper;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Auth0.ManagementApi.Models;
using Azure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Gatam.Infrastructure.Contexts;

namespace Gatam.Infrastructure.Repositories;

public class ManagementApiRepository: IManagementApi
{
    private readonly HttpClient _httpClient;
    private readonly EnvironmentWrapper _environmentWrapper;
    private readonly IUnitOfWork _unitOfWork;
    public ManagementApiRepository(IUnitOfWork uow,HttpClient httpClient, EnvironmentWrapper environmentWrapper)
    {
        _httpClient = httpClient;
        _unitOfWork = uow;
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
                Name = user.TryGetProperty("name", out var name) ? name.GetString() : string.Empty,
                Surname = user.TryGetProperty("surname", out var surname) ? surname.GetString() : string.Empty,
                Username = user.TryGetProperty("username", out var username) ? username.GetString() : string.Empty,
                Email = user.GetProperty("email").GetString(),
                PhoneNumber=user.GetProperty("phoneNumber").GetString(),
                IsActive = !user.TryGetProperty("blocked", out var blocked) || !blocked.GetBoolean(),
                Picture = user.TryGetProperty("picture", out var picture) ? picture.GetString() : null,
                RolesIds = new List<string>()
            };

            userDto.RolesIds = (await GetRolesByUserId(userId)).ToList();
            userDtos.Add(userDto);
        }

        return userDtos;
    }

    public async Task<UserDTO> GetUserByIdAsync(string userId)
    {
        var _response = await _httpClient.GetFromJsonAsync<JsonElement>($"users/{userId}");
        UserDTO _user = new UserDTO
        {
            Id = userId,
            Email = _response.GetProperty("email").GetString(),
            Name = _response.TryGetProperty("name", out var name) ? name.GetString() : string.Empty,
            Surname = _response.TryGetProperty("surname", out var surname) ? surname.GetString() : string.Empty,
            Username = _response.TryGetProperty("username", out var username) ? username.GetString() : string.Empty,
            PhoneNumber=_response.GetProperty("phoneNumber").GetString(),
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

            name = user.Name,
            nickname = user.Surname,
            username = user.Username,
            email = user.Email,
            password = user.PasswordHash,
            connection = "Username-Password-Authentication",
            user_metadata = new
            {
                phonenumber = user.PhoneNumber,
                picture = user.Picture,
            },

        };
        
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v2/users", payload);

            if (response.IsSuccessStatusCode)
            {
                var createdUser = await response.Content.ReadFromJsonAsync<Auth0ResponseUsers>();


                if (createdUser == null)
                {
                    return null;
                }

                string userId = createdUser.UserId;
                

                var applicationUser = new ApplicationUser
                {
                    Id = userId,  
                    Name = createdUser.Name,
                    Surname = createdUser.Nickname,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    PhoneNumber = createdUser.UserMetadata?.PhoneNumber,
                    Picture = createdUser.UserMetadata?.Picture,
                    RolesIds = RoleMapper.GetRoleValues("VOLGER"),
                    IsActive = true 
                };

                

                return applicationUser;
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

    public async Task<bool> DeleteUserAsync(string userId)
    {        
       var response = await _httpClient.DeleteAsync($"/api/v2/users/{userId}");
       return response.IsSuccessStatusCode;
    }

    public async Task<UserDTO> UpdateUserAsync(UserDTO user)
    {
        var payload = new
        {
            name = user.Username,
            email=user.Email,
            username = user.Username

        };

        var _userId = user.Id;

        var response = await _httpClient.PatchAsJsonAsync($"/api/v2/users/{_userId}", payload);


        if (response.IsSuccessStatusCode)
        {
            return user;
            
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error updating user: {response.StatusCode} - {errorDetails}");
        return null;
    }

    public async Task<UserDTO> UpdateUserNicknameAsync(UserDTO user)
    {
        var payload = new
        {
            nickname = user.Username,
            username = user.Username,  
        };

        var _userId = user.Id;

        var response = await _httpClient.PatchAsJsonAsync($"/api/v2/users/{_userId}", payload);


        if (response.IsSuccessStatusCode)
        {
            return user;
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error updating nickname: {response.StatusCode} - {errorDetails}");
        return null;
    }

    public async Task<UserDTO> UpdateUserEmailAsync(UserDTO user)
    {
        var payload = new
        {
            email = user.Email,  
        };

        var _userId = user.Id;

        var response = await _httpClient.PatchAsJsonAsync($"/api/v2/users/{_userId}", payload);


        if (response.IsSuccessStatusCode)
        {
            return user;
        }

        var errorDetails = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Error updating email: {response.StatusCode} - {errorDetails}");
        return null;
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

        
        
        var response = await _httpClient.PostAsJsonAsync($"/api/v2/users/{user.Id}/roles", payload);

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
