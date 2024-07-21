using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FFP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Extract the embedded .jar file
            var assembly = Assembly.GetExecutingAssembly();
            var jarName = "FFP.jar"; // Change 'yourJarFile.jar' to the actual name of your JAR file
            var tempJarPath = Path.Combine(Path.GetTempPath(), jarName);
            var name = assembly.GetManifestResourceNames()[0];
            using (var stream = assembly.GetManifestResourceStream(name)) // Adjust namespace if necessary

            using (var fileStream = new FileStream(tempJarPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
            var javaPath = "java";
            // Construct the argument list to pass to Java
            var jarArguments = $"-jar \"{tempJarPath}\" " + string.Join(" ", args.Select(arg => $"\"{arg}\""));

            var processInfo = new ProcessStartInfo(javaPath, jarArguments)
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                WindowStyle = ProcessWindowStyle.Normal,
                RedirectStandardInput = true,
            };

            var process = Process.Start(processInfo);
            process.WaitForExit();
            process.Close();
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
