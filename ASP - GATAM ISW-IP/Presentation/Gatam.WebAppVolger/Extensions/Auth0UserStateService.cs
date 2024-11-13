using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace Gatam.WebAppVolger.Extensions
{
    public class Auth0UserStateService
    {
        private readonly ApiClient _httpClient;

        public Auth0UserStateService(ApiClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsUserInactive(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return false; // User is not logged in
            }

            try
            {
                var response = await _httpClient.GetFromJsonAsync<JsonElement>($"/api/user/status/{userId}");

                if (response.TryGetProperty("isActive", out JsonElement isActiveElement))
                {
                    bool isActive = isActiveElement.GetBoolean();
                    Debug.WriteLine($"IsActive status retrieved: {isActive}");
                    return !isActive; // Return true if user is inactive
                }
                else
                {
                    Debug.WriteLine("isActive property is missing or has an unexpected value.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}