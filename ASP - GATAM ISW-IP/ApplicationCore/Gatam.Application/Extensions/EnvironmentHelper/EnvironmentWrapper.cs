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
        public string APIKEY { get; set; }

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
            APIKEY = Environment.GetEnvironmentVariable("API_KEY")?? "";

            /// NULL CHECKS
            if (string.IsNullOrEmpty(DATABASEHOST)) { throw new MissingEnvironmentVariableException(nameof(DATABASEHOST)); }
            if (string.IsNullOrEmpty(DATABASENAME)) { throw new MissingEnvironmentVariableException(nameof(DATABASENAME)); }
            if (string.IsNullOrEmpty(DATABASEUSER)) { throw new MissingEnvironmentVariableException(nameof(DATABASEUSER)); }
            if (string.IsNullOrEmpty(SAPASSWORD)) { throw new MissingEnvironmentVariableException(nameof(SAPASSWORD)); }
            if (string.IsNullOrEmpty(AUTH0DOMAIN)) { throw new MissingEnvironmentVariableException(nameof(AUTH0DOMAIN)); }
            if (string.IsNullOrEmpty(AUTH0AUDIENCE)) { throw new MissingEnvironmentVariableException(nameof(AUTH0AUDIENCE)); }
            if (string.IsNullOrEmpty(TOKEN)) { throw new MissingEnvironmentVariableException(nameof(TOKEN)) }  
            if (string.IsNullOrEmpty(BASEURI)) { throw new MissingEnvironmentVariableException(nameof(BASEURI)); }
            if (string.IsNullOrEmpty(KEY)) { throw new MissingEnvironmentVariableException(nameof(KEY)); }
            if (string.IsNullOrEmpty(APIKEY)) { throw new MissingEnvironmentVariableException(nameof(APIKEY)); }

            // VALID CHECKS
            if (!DATABASEHOST.Contains(",")) { throw new InvalidEnvironmentVariableException($"{DATABASEHOST} missing port seperator"); }
            Match m = DotEnvLoader.ValidateWithExpression("/(,\\d{4})$/g", DATABASEHOST);
            if (m.Success) { throw new InvalidEnvironmentVariableException($"{DATABASEHOST} Invalid port. Check your environment file..."); }
        }


    }
}
