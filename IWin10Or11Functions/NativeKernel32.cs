using System.Runtime.InteropServices;

namespace IWin10Or11Functions
{
    internal class NativeKernel32
    {
        private const string KERNEL32_DLL = "kernel32.dll";

        [DllImport(KERNEL32_DLL, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetComputerNameW([Out] char[] lpBuffer, ref uint nSize);
    }
}
