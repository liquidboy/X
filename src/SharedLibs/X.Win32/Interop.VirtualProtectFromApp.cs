using System;
using System.Runtime.InteropServices;

namespace X.Win32
{
    internal partial class Interop
    {
        [DllImport(NativeLib.Memory_L1_3, CharSet = CharSet.Unicode, SetLastError = true)]
        internal extern static IntPtr VirtualProtectFromApp(
            SafeHandle Address,
            UIntPtr Size,
            int NewProtection,
            out IntPtr OldProtection);

    }
}


//https://msdn.microsoft.com/en-us/library/windows/desktop/mt169846(v=vs.85).aspx
//http://stackoverflow.com/questions/7473202/dynamic-code-execution-on-winrt-in-windows-8-either-c-or-net-c/33729808#33729808