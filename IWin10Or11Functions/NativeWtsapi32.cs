using System.Runtime.InteropServices;

namespace IWin10Or11Functions
{
    internal class NativeWtsapi32
    {
        private const string WTSAPI32_DLL = "wtsapi32.dll";

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        public enum WTS_INFO_CLASS
        {
            WTSUserName = 5,
            WTSDomainName = 7,
            WTSConnectState = 8,
            WTSIdleTime = 17,
            WTSLogonTime = 18
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WTS_SESSION_INFOW
        {
            public uint SessionId;
            public IntPtr pWinStationName;
            public WTS_CONNECTSTATE_CLASS State;
        }

        [DllImport(WTSAPI32_DLL)]
        public static extern void WTSCloseServer(IntPtr hServer);

        [DllImport(WTSAPI32_DLL, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WTSEnumerateSessionsW(IntPtr hServer, uint Reserved, uint Version,
            out IntPtr ppSessionInfo, out uint pCount);

        [DllImport(WTSAPI32_DLL, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr WTSOpenServerW([MarshalAs(UnmanagedType.LPWStr)] string pServerName);

        [DllImport(WTSAPI32_DLL, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr WTSOpenServerExW([MarshalAs(UnmanagedType.LPWStr)] string pServerName);

        [DllImport(WTSAPI32_DLL, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WTSQuerySessionInformationW(IntPtr hServer, uint SessionId, WTS_INFO_CLASS WTSInfoClass,
            out IntPtr ppBuffer, out uint pBytesReturned);

        [DllImport(WTSAPI32_DLL)]
        public static extern void WTSFreeMemory(IntPtr pMemory);
    }
}
