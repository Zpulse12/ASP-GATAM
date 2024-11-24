using Gatam.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    public interface IHttpWrapper
    {
        Task<Result<HttpResponseMessage>> SendDeleteWithBody<T>(string url,T body);
    }
}
