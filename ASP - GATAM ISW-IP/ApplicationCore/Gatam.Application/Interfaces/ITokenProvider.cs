namespace Gatam.Application.Interfaces
{
    public interface ITokenProvider
    {
        public Task<string> GetToken();
    }
}
