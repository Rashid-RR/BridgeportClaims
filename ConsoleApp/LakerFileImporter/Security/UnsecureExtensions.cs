using System;
using System.Runtime.InteropServices;
using System.Security;

namespace LakerFileImporter.Security
{
    public static class UnsecureExtensions
    {
        public static string ToUnsecureString(this SecureString securePassword)
        {
            if (null == securePassword)
                throw new ArgumentNullException(nameof(securePassword));
            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
