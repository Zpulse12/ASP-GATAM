using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.WebAppBegeleider.Extensions.EnvironmentHelper
{
    public static class SolutionWrapper
    {

        public static DirectoryInfo GetSolutionDirectoryPath(string currentPath = null)
        {
            DirectoryInfo solutionDirectory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
            while (solutionDirectory != null && !solutionDirectory.GetFiles("*.sln").Any())
            {
                solutionDirectory = solutionDirectory.Parent;
            }
            return solutionDirectory;
        }

    }
}
