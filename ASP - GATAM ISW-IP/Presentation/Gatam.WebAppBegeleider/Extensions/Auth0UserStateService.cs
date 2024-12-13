using System.Security.Claims;
using System.Text.Json;

namespace Gatam.WebAppBegeleider.Extensions
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

            return false;
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
                    return !isActive; // Return true if user is inactive
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}