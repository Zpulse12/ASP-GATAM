using System.Net.Http.Json;
using System.Text.Json;
using Gatam.Application.CQRS;
using Gatam.Application.Extensions.EnvironmentHelper;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Application.Extensions.httpExtensions;
using Gatam.Application.CQRS.DTOS.RolesDTO;
using Azure.Core;

namespace Gatam.Infrastructure.Repositories;

public class ManagementApiRepository: IManagementApi
{
    private readonly HttpClient _httpClient;
    private readonly EnvironmentWrapper _environmentWrapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpWrapper _httpWrapper;
    private readonly string _baseURL;
    public ManagementApiRepository(IUnitOfWork uow,HttpClient httpClient, EnvironmentWrapper environmentWrapper)
    {
        _httpClient = httpClient;
        _unitOfWork = uow;
        _environmentWrapper = environmentWrapper;
        _httpClient.BaseAddress = new Uri(_environmentWrapper.BASEURI);
        _httpClient.DefaultRequestHeaders.Add("Authorization", @$"Bearer {_environmentWrapper.TOKEN}");
        _httpWrapper = new HttpWrapper(_httpClient);
        _baseURL = _environmentWrapper.BASEURI;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        try 
        {
            var response = await _httpClient.GetFromJsonAsync<JsonElement>("users");
            return response.EnumerateArray().Select(user => new UserDTO
            {
                Id = user.TryGetProperty("user_id", out var id) ? id.GetString() : string.Empty,
                Username = user.TryGetProperty("username", out var username) ? username.GetString() : string.Empty,
                Email = user.TryGetProperty("email", out var email) ? email.GetString() : string.Empty,
                Name = user.TryGetProperty("name", out var name) ? name.GetString() : string.Empty,
                Surname = user.TryGetProperty("nickname", out var nickname) ? nickname.GetString() : string.Empty,
                Picture = user.TryGetProperty("picture", out var picture) ? picture.GetString() : string.Empty,
                PhoneNumber = user.TryGetProperty("phone_number", out var phone) ? phone.GetString() : string.Empty,
                RolesIds = user.TryGetProperty("roles", out var roles) ? roles.EnumerateArray().Select(role => role.GetProperty("name").GetString()).ToList() : new List<string>()
            }).ToList();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<UserDTO> GetUserByIdAsync(string userId)
    {
        var _response = await _httpClient.GetFromJsonAsync<JsonElement>($"users/{userId}");
        UserDTO _user = new UserDTO
        {
            Id = userId,
            Name = _response.TryGetProperty("name", out var name) ? name.GetString() : string.Empty,
            Surname = _response.TryGetProperty("surname", out var surname) ? surname.GetString() : string.Empty,
            Username = _response.TryGetProperty("username", out var username) ? username.GetString() : string.Empty,
            Email = _response.TryGetProperty("email", out var email) ? username.GetString() : string.Empty,
            PhoneNumber = _response.TryGetProperty("phoneNumber", out var phoneNumber) ? phoneNumber.GetString() : string.Empty,
            IsActive = !_response.TryGetProperty("blocked", out var blocked) || !blocked.GetBoolean(),
           // RolesIds = new List<string>()
        };
        return _user; 
    }
    public async Task<Result<ApplicationUser>> CreateUserAsync(ApplicationUser user)
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
                    return Result<ApplicationUser>.Fail(new Exception("Could not convert auth response to Auth0ResponseUsers object"));
                } 

                var applicationUser = new ApplicationUser
                {
                    Id = createdUser.UserId,  
                    Name = createdUser.Name,
                    Surname = createdUser.Nickname,
                    Username = createdUser.Username,
                    Email = createdUser.Email,
                    PasswordHash = user.PasswordHash,
                    Picture = createdUser.UserMetadata?.Picture,
                    BegeleiderId = user.BegeleiderId,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = true,
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            RoleId = RoleMapper.Roles[CustomRoles.VOLGER].Id
                        }
                    }
                };

                return Result<ApplicationUser>.Ok(applicationUser);
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return Result<ApplicationUser>.Fail(new Exception(errorDetails));
            }
        }
        catch (Exception ex)
        {
            return Result<ApplicationUser>.Fail(ex);
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

    public async Task<Result<bool>> UpdateUserRoleAsync(string userId, RolesDTO roles)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/v2/users/{userId}/roles", roles);
            if (!response.IsSuccessStatusCode)
            {
                return Result<bool>.Fail(new Exception(response.ReasonPhrase));
            }

            var user = await _unitOfWork.UserRepository.FindById(userId);
            if (user == null)
            {
                return Result<bool>.Fail(new Exception("User not found"));
            }

            foreach (var roleId in roles.Roles)
            {
                if (!user.UserRoles.Any(ur => ur.RoleId == roleId))
                {
                    user.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
                }
            }

            await _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Commit();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail(ex);
        }
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
                               .Select(name => RoleMapper.Roles.FirstOrDefault(role => role.Value.Name == name).Value.Name)
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

    public async Task<Result<bool>> DeleteUserRoles(string userId, RolesDTO roles)
    {
        Result<HttpResponseMessage> request =  await _httpWrapper.SendDeleteWithBody<RolesDTO>( $"{_baseURL}users/{userId}/roles", roles);
        if (request.Success) { 
            return Result<bool>.Ok(true);
        } else
        {
            return Result<bool>.Fail(request.Exception);
        }
    }
}
