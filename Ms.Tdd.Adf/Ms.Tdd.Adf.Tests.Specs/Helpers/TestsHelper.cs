using System.Reflection;
using System.Resources;
using System.Text;

namespace Ms.Tdd.Adf.Tests.Specs.Helpers
{
    internal static class TestsHelper
    {
        internal static string? GetResourceFileContents(string resourceFileName)
        {
            var resourceManager = new ResourceManager("Ms.Tdd.Adf.Tests.Specs.Resources.TestResources", Assembly.GetExecutingAssembly());
            return resourceManager.GetString(resourceFileName);
        }

        internal static Stream MakeFileStream(string fileContents)
        {
            var fileBytes = Encoding.UTF8.GetBytes(fileContents);
            return new MemoryStream(fileBytes);
        }

        internal static Stream GetFileStreamForResourceFile(string resourceFileName)
        {
            var sampleDataFile = GetResourceFileContents(resourceFileName);
            return MakeFileStream(sampleDataFile ?? string.Empty);
        }
    }
}