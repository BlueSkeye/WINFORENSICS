using INative;
using System.Runtime.InteropServices;
using static IWin10Or11Functions.NativeWtsapi32;

namespace IWin10Or11Functions
{
    public class _Server : IFunctions
    {
        /// <summary>The server handle that is valid for the duration of the object's lifetime.
        /// Initialized in the constructor.</summary>
        private IntPtr _hServer;

        /// <summary>The default constructor for the _Server class.</summary>
        public _Server() : this(RunningApplicationServerName()) { }

        /// <summary>The constructor for the _Server class with a specified server name.</summary>
        /// <param name="currentServerName">Default parameter value is the current server name.</param>
        public _Server(string currentServerName)
        {
            IntPtr serverHandle = NativeWtsapi32.WTSOpenServerExW(currentServerName);
            _hServer = serverHandle;
        }

        /// <summary>On deletion make sure to close the server handle and zeroize it.</summary>
        ~_Server()
        {
            if (IntPtr.Zero != _hServer) {
                NativeWtsapi32.WTSCloseServer(_hServer);
                _hServer = IntPtr.Zero;
            }
        }

        public IEnumerable<NativeWtsapi32.WTS_SESSION_INFOW> EnumerateSessions()
        {
            uint sessionCount;
            IntPtr ppSessionInfo;
            if (!NativeWtsapi32.WTSEnumerateSessionsW(_hServer, 0, 1, out ppSessionInfo, out sessionCount)) {
                throw new ApplicationException("Failed to enumerate sessions.");
            }
            try {
                int dataSize = Marshal.SizeOf(typeof(NativeWtsapi32.WTS_SESSION_INFOW));
                long ppSessionInfoLong = ppSessionInfo.ToInt64();
                for (uint enumerationIndex = 0; enumerationIndex < sessionCount; enumerationIndex++) {
                    IntPtr currentSessionInfoPtr = new IntPtr(ppSessionInfoLong + (enumerationIndex * dataSize));
                    NativeWtsapi32.WTS_SESSION_INFOW sessionInfo =
                        Marshal.PtrToStructure<NativeWtsapi32.WTS_SESSION_INFOW>(currentSessionInfoPtr);
                    // Process the session information as needed
                    yield return sessionInfo;
                }
                yield break;
            }
            finally {
                NativeWtsapi32.WTSFreeMemory(ppSessionInfo);
                // TODO : Handle exception if any.
            }
            // Never reached.
        }

        public Dictionary<int, string> GetAllSessionInformation()
        {
            Dictionary<int, string> baseDictionnary = new Dictionary<int, string>();

            // Enumerate all WTS_INFO_CLASS values and process them for a base dictionnary.
            foreach (NativeWtsapi32.WTS_INFO_CLASS informationClassValue in Enum.GetValues(typeof(NativeWtsapi32.WTS_INFO_CLASS))) {
                string? informationName = Enum.GetName(typeof(NativeWtsapi32.WTS_INFO_CLASS), informationClassValue);
                if (null == informationName) {
                    continue;
                }
                baseDictionnary.Add((int)informationClassValue, informationName);
            }

            Dictionary<int, string> thisDictionnary = new Dictionary<int, string>();
            foreach (NativeWtsapi32.WTS_SESSION_INFOW information in EnumerateSessions()) {
                foreach(KeyValuePair<int, string> pair in baseDictionnary) {
                    IntPtr ppCurrentSessionInfoPtr = IntPtr.Zero;
                    uint pBytesReturned = 0;
                    if (!NativeWtsapi32.WTSQuerySessionInformationW(_hServer, information.SessionId,
                        (NativeWtsapi32.WTS_INFO_CLASS)pair.Key, out ppCurrentSessionInfoPtr, out pBytesReturned))
                    {
                        // The required information is not defined. Skip it.
                        continue;
                    }
                    Console.WriteLine($"K={pair.Key}/V={ppCurrentSessionInfoPtr}+{pBytesReturned}");
                    string thisString = new string(Marshal.PtrToStringUni((nint)ppCurrentSessionInfoPtr.ToInt64()) ?? string.Empty);
                    thisDictionnary.Add(pair.Key, thisString);
                }
            }
            return thisDictionnary;
        }

        public static IntPtr RunningApplicationHandle() => RunningApplicationHandle(RunningApplicationServerName());

        public static IntPtr RunningApplicationHandle(string currentServerName)
        {
            if (string.IsNullOrEmpty(currentServerName)) {
                throw new ArgumentException("Current server name cannot be null or empty.");
            }
            return NativeWtsapi32.WTSOpenServerW(currentServerName);
            // Here you can implement logic to handle the running application on the specified server.
            // For example, you might want to establish a connection or perform some operations.
        }

        /// <summary>Get and return the name of the PC or server on which the application is currently running.</summary>
        /// <returns></returns>
        public static string RunningApplicationServerName()
        {
            const int MAX_COMPUTERNAME_LENGTH = 256;
            uint length = MAX_COMPUTERNAME_LENGTH;
            char[] localBuffer = new char[length + 1];

            if (!NativeKernel32.GetComputerNameW(localBuffer, ref length)) {
                Errors.Code currentErrcode = NativeUcrtbase.Errno;
                switch (currentErrcode) {
                    case Errors.Code.ERROR_BUFFER_OVERFLOW:
                        throw new ApplicationException("Buffer overflow occurred while retrieving computer name.\n");
                    default:
                        throw new ApplicationException($"Unknown error occurred. Error code: {currentErrcode}\n");
                }
            }
            return new string(localBuffer, 0, (int)length);
        }
    }
}
