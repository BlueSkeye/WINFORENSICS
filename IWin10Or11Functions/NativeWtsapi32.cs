using System.Runtime.InteropServices;

namespace IWin10Or11Functions
{
    /// <summary>This class is public because it uses some enumeration and structure definitions that
    /// are needed by the ultimate calling code outside of this assembly. Later this class will be made
    /// internal when returned values remain in this assembly.</summary>
    public class NativeWtsapi32
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

        /// <summary>See : https://learn.microsoft.com/en-us/windows/win32/api/wtsapi32/ne-wtsapi32-wts_info_class</summary>
        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram = 0,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName = 5,
            WTSWinStationName,
            WTSDomainName = 7,
            WTSConnectState = 8,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType,
            WTSIdleTime = 17,
            WTSLogonTime = 18,
            WTSIncomingBytes,
            WTSOutgoingBytes,
            WTSIncomingFrames,
            WTSOutgoingFrames,
            WTSClientInfo,
            WTSSessionInfo,
            WTSSessionInfoEx,
            WTSConfigInfo,
            WTSValidationInfo,
            WTSSessionAddressV4,
            WTSIsRemoteSession,
            WTSSessionActivityId,
            WTSCapabilityCheck
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WTS_SESSION_INFOW
        {
            public uint SessionId;
            public IntPtr pWinStationName;
            public WTS_CONNECTSTATE_CLASS State;

            public string? WinStationName
            {
                get {
                    return (nint.Zero != pWinStationName) ? Marshal.PtrToStringUni(pWinStationName) : null;
                }
            }
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

        /// <summary>See :
        /// https://learn.microsoft.com/en-us/windows/win32/api/wtsapi32/nf-wtsapi32-wtsquerysessioninformationw</summary>
        [DllImport(WTSAPI32_DLL, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WTSQuerySessionInformationW(IntPtr hServer, uint SessionId, WTS_INFO_CLASS WTSInfoClass,
            out IntPtr ppBuffer, out uint pBytesReturned);

        [DllImport(WTSAPI32_DLL)]
        public static extern void WTSFreeMemory(IntPtr pMemory);
    }
}
