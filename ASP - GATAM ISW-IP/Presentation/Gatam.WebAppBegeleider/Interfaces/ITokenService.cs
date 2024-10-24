namespace Gatam.WebAppBegeleider.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetBearerTokenAsync();
    }
}
