using System.Text.RegularExpressions;

namespace Gatam.WebAppBegeleider.Extensions.EnvironmentHelper
{
    public static class DotEnvLoader
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }

        public static Match ValidateWithExpression(string expression, string text)
        {
            Regex reg = new Regex(expression, RegexOptions.IgnoreCase);
            return reg.Match(text);
        }

    }
}
