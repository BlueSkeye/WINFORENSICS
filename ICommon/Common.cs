namespace ICommon
{
    public interface Common
    {
        /// <summary>Will return a TargetEnvironment enumeration value that will define the target environment.
        /// It will be used later to load the appropriate interface under a common Ix abstraction.</summary>
        /// <exception cref="NotSupportedException"></exception>
        public static TargetEnvironment Environment()
        {
            if (OperatingSystem.IsWindows()) {
                return TargetEnvironment.Windows;
            }
            if (OperatingSystem.IsLinux()) {
                return TargetEnvironment.Linux;
            }
            throw new NotSupportedException("Operating system is not supported.");
        }

    }
}
