using INative;

namespace ComLocCon
{
    internal enum TargetEnvironment
    {
        Windows,
        Linux
    };

    /// <summary>A common localization application able to run on a Windows (10 or 11) or LINUX OS.
    /// Will discover and load the Windows (10 or 11) or LINUX OS common interface.
    /// Starting from there, a unified interface running shouls be used.</summary>
    internal class Program
    {
        /// <summary>Will return a TargetEnvironment enumeration value that will define the target environment.
        /// It will be used later to load the appropriate interface under a common Ix abstraction.</summary>
        /// <exception cref="NotSupportedException"></exception>
        private static TargetEnvironment GetTargetEnvironment()
        {
            if (OperatingSystem.IsWindows()) {
                return TargetEnvironment.Windows;
            }
            if (OperatingSystem.IsLinux()) {
                return TargetEnvironment.Linux;
            }
            throw new NotSupportedException("Operating system is not supported.");
        }

        /// <summary>Main static method of this program.</summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Define target environment. Will always return a valid value or throw an exception before returning.
            TargetEnvironment targetEnvironment = GetTargetEnvironment();

            Console.WriteLine("Hello, World!");
        }
    }
}
