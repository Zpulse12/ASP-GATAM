using Gatam.Application.Extensions;

namespace Gatam.Application.Interfaces
{
    public interface IHttpWrapper
    {
        Task<Result<HttpResponseMessage>> SendDeleteWithBody<T>(string url,T body);
    }
}
