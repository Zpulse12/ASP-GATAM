using Gatam.Infrastructure.Exceptions;
using Gatam.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gatam.Application.Extensions
{
    public class EnvironmentWrapper
    {
        public string SAPASSWORD { get; set; }
        public string DATABASENAME { get; set; }
        public string DATABASEHOST { get; set; }
        public string DATABASEUSER { get; set; }
        public string AUTH0DOMAIN { get; set; }
        public string AUTH0CLIENTID { get; set; }
        public string AUTH0CLIENTSECRET { get; set; }
        public string AUTH0AUDIENCE { get; set; }
        public string TOKEN { get; set; }
        public string BASEURI { get; set; }

        public EnvironmentWrapper() {
#if DEBUG
            DirectoryInfo rootDirectory = SolutionWrapper.GetSolutionDirectoryPath();
            string dotenvPath = Path.Combine(rootDirectory.FullName, ".env");
            DotEnvLoader.Load(dotenvPath);
#endif
            SAPASSWORD = Environment.GetEnvironmentVariable("SA_PASSWORD") ?? "";
            DATABASENAME = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "";
            DATABASEHOST = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "";
            DATABASEUSER = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "";
            AUTH0DOMAIN = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? "";
            AUTH0CLIENTID = Environment.GetEnvironmentVariable("AUTH0_CLIENTID") ?? "";
            AUTH0CLIENTSECRET = Environment.GetEnvironmentVariable("AUTH0_CLIENTSECRET") ?? "";
            AUTH0AUDIENCE = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE") ?? "";
            TOKEN = Environment.GetEnvironmentVariable("TOKEN") ?? "";
            BASEURI = Environment.GetEnvironmentVariable("BASE_URI") ?? "";
            
            /// NULL CHECKS
            if (string.IsNullOrEmpty(DATABASEHOST)) { throw new MissingEnvironmentVariableException(nameof(DATABASEHOST)); }
            if (string.IsNullOrEmpty(DATABASENAME)) { throw new MissingEnvironmentVariableException(nameof(DATABASENAME)); }
            if (string.IsNullOrEmpty(DATABASEUSER)) { throw new MissingEnvironmentVariableException(nameof(DATABASEUSER)); }
            if (string.IsNullOrEmpty(SAPASSWORD)) { throw new MissingEnvironmentVariableException(nameof(SAPASSWORD)); }
            if (string.IsNullOrEmpty(AUTH0DOMAIN)) { throw new MissingEnvironmentVariableException(nameof(AUTH0DOMAIN)); }
            if (string.IsNullOrEmpty(AUTH0CLIENTID)) { throw new MissingEnvironmentVariableException(nameof(AUTH0CLIENTID)); }
            if (string.IsNullOrEmpty(AUTH0CLIENTSECRET)) { throw new MissingEnvironmentVariableException(nameof(AUTH0CLIENTSECRET)); }
            if (string.IsNullOrEmpty(AUTH0AUDIENCE)) { throw new MissingEnvironmentVariableException(nameof(AUTH0AUDIENCE)); }
            if (string.IsNullOrEmpty(TOKEN)) { throw new MissingEnvironmentVariableException(nameof(TOKEN)); }
            if (string.IsNullOrEmpty(BASEURI)) { throw new MissingEnvironmentVariableException(nameof(BASEURI)); }
            // VALID CHECKS
            if (!DATABASEHOST.Contains(",")) { throw new InvalidEnvironmentVariableException($"{nameof(DATABASEHOST)} missing port seperator"); }
            Match m = DotEnvLoader.ValidateWithExpression("/(,\\d{4})$/g", DATABASEHOST);
            if (m.Success) { throw new InvalidEnvironmentVariableException($"{nameof(DATABASEHOST)} Invalid port. Check your environment file..."); }
        }


    }
}
