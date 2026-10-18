using System.Runtime.InteropServices;

namespace IWin10Or11Functions
{
    internal class NativeUcrtbase
    {
        private const string UCRTBASE_DLL = "ucrtbase.dll";

        /// <summary>Gets the error number for the last error that occurred.</summary>
        public static Errors.Code Errno
        {
            get
            {
                int returnValue;
                
                _get_errno(out returnValue);
                return (Errors.Code)returnValue;
            }
        }

        [DllImport(UCRTBASE_DLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _get_errno(out int pValue);
    }
}
