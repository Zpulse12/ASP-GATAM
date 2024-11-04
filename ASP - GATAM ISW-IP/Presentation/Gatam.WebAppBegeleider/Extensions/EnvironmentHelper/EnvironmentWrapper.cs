using Gatam.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gatam.WebAppBegeleider.Extensions.EnvironmentHelper
{
    public class EnvironmentWrapper
    {
        public string AUTH0DOMAIN { get; set; }
        public string AUTH0CLIENTID { get; set; }
        public string AUTH0CLIENTSECRET { get; set; }
        public string AUTH0AUDIENCE { get; set; }

        public string ENVIRONMENT { get; set; }
        public EnvironmentWrapper()
        {
#if DEBUG
            DirectoryInfo rootDirectory = SolutionWrapper.GetSolutionDirectoryPath();
            string dotenvPath = Path.Combine(rootDirectory.FullName, "debug.env");
            DotEnvLoader.Load(dotenvPath);
#endif
            AUTH0DOMAIN = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? "";
            AUTH0CLIENTID = Environment.GetEnvironmentVariable("AUTH0_CLIENTID") ?? "";
            AUTH0CLIENTSECRET = Environment.GetEnvironmentVariable("AUTH0_CLIENTSECRET") ?? "";
            AUTH0AUDIENCE = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE") ?? "";
            ENVIRONMENT = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "";

            /// NULL CHECKS
            if (string.IsNullOrEmpty(AUTH0DOMAIN)) { throw new MissingEnvironmentVariableException(nameof(AUTH0DOMAIN)); }
            if (string.IsNullOrEmpty(AUTH0CLIENTID)) { throw new MissingEnvironmentVariableException(nameof(AUTH0CLIENTID)); }
            if (string.IsNullOrEmpty(AUTH0CLIENTSECRET)) { throw new MissingEnvironmentVariableException(nameof(AUTH0CLIENTSECRET)); }
            if (string.IsNullOrEmpty(AUTH0AUDIENCE)) { throw new MissingEnvironmentVariableException(nameof(AUTH0AUDIENCE)); }
            if (string.IsNullOrEmpty(ENVIRONMENT)) { throw new MissingEnvironmentVariableException(nameof(ENVIRONMENT)); }
        }
    }
}
