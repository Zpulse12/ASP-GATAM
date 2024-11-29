using Gatam.Infrastructure.Exceptions;
using System.Text.RegularExpressions;

namespace Gatam.Application.Extensions.EnvironmentHelper
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
        public string KEY { get; set; }

        public EnvironmentWrapper()
        {
#if DEBUG
            DirectoryInfo rootDirectory = SolutionWrapper.GetSolutionDirectoryPath();
            string dotenvPath = Path.Combine(rootDirectory.FullName, "debug.env");
            DotEnvLoader.Load(dotenvPath);
#endif
            SAPASSWORD = Environment.GetEnvironmentVariable("SA_PASSWORD") ?? "";
            DATABASENAME = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "";
            DATABASEHOST = Environment.GetEnvironmentVariable("DATABASE_HOST") ?? "";
            DATABASEUSER = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "";
            AUTH0DOMAIN = Environment.GetEnvironmentVariable("AUTH0_DOMAIN") ?? "";
            AUTH0AUDIENCE = Environment.GetEnvironmentVariable("AUTH0_AUDIENCE") ?? "";
            TOKEN = Environment.GetEnvironmentVariable("TOKEN") ?? "";
            BASEURI = Environment.GetEnvironmentVariable("BASE_URI") ?? "";
            KEY = Environment.GetEnvironmentVariable("KEY")?? "";

            /// NULL CHECKS
            if (string.IsNullOrEmpty(DATABASEHOST)) { throw new MissingEnvironmentVariableException(DATABASEHOST); }
            if (string.IsNullOrEmpty(DATABASENAME)) { throw new MissingEnvironmentVariableException(DATABASENAME); }
            if (string.IsNullOrEmpty(DATABASEUSER)) { throw new MissingEnvironmentVariableException(DATABASEUSER); }
            if (string.IsNullOrEmpty(SAPASSWORD)) { throw new MissingEnvironmentVariableException(SAPASSWORD); }
            if (string.IsNullOrEmpty(AUTH0DOMAIN)) { throw new MissingEnvironmentVariableException(AUTH0DOMAIN); }
            if (string.IsNullOrEmpty(AUTH0AUDIENCE)) { throw new MissingEnvironmentVariableException(AUTH0AUDIENCE); }
            if (string.IsNullOrEmpty(TOKEN)) { throw new MissingEnvironmentVariableException(TOKEN); }  
            if (string.IsNullOrEmpty(BASEURI)) { throw new MissingEnvironmentVariableException(BASEURI); }
            if (string.IsNullOrEmpty(KEY)) { throw new MissingEnvironmentVariableException(KEY); }

            // VALID CHECKS
            if (!DATABASEHOST.Contains(",")) { throw new InvalidEnvironmentVariableException($"{DATABASEHOST} missing port seperator"); }
            Match m = DotEnvLoader.ValidateWithExpression("/(,\\d{4})$/g", DATABASEHOST);
            if (m.Success) { throw new InvalidEnvironmentVariableException($"{DATABASEHOST} Invalid port. Check your environment file..."); }
        }


    }
}
