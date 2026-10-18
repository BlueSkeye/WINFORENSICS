using INative;

namespace IWin10Or11Functions
{
    public class _Server : IFunctions
    {
        private IntPtr _hServer;

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

        /// <summary>The default constructor for the _Server class.</summary>
        public _Server() : this(RunningApplicationServerName()) { }

        public _Server(string currentServerName)
        {
            IntPtr serverHandle = NativeWtsapi32.WTSOpenServerW(currentServerName);
            _hServer = serverHandle;
        }
    }
}
