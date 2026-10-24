using ICommon;
// Because we are not handling Linux functions yet.
using IWin10Or11Functions;

namespace ComLocCon
{
    /// <summary>A common localization application able to run on a Windows (10 or 11) or LINUX OS.
    /// Will discover and load the Windows (10 or 11) or LINUX OS common interface.
    /// Starting from there, a unified interface running shouls be used.</summary>
    internal class Program
    {
        /// <summary>Main static method of this program.</summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Define target environment. Will always return a valid value or throw an exception before returning.
            ICommon.TargetEnvironment targetEnvironment = Common.Environment();

            Console.WriteLine($"Target environment is : {targetEnvironment}.\n");
            // Safeguard against Linux OS handling - not yet implemented.
            if (ICommon.TargetEnvironment.Linux == targetEnvironment) {
                Console.WriteLine("Linux OS is not yet supported. Exiting.");
                Environment.Exit(0);
            }

            if (ICommon.TargetEnvironment.Windows == targetEnvironment) {
                string currentServerName = _Server.RunningApplicationServerName();
                _Server thisServer = new _Server(currentServerName);
                foreach (var sessionInfo in thisServer.EnumerateSessions()) {
                    Console.WriteLine($"Session ID: {sessionInfo.SessionId} ({sessionInfo.WinStationName}), State: {sessionInfo.State}");
                    Dictionary<int, string> allSessionInformation = thisServer.GetAllSessionInformation();
                    foreach (KeyValuePair<int, string> pair in allSessionInformation) {
                        Console.WriteLine($"\t{pair.Key}: {pair.Value}");
                    }
                }
                Environment.Exit(0);
            }

            Environment.Exit(0);
        }
    }
}
