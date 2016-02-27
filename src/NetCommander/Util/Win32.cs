using System;
using System.Runtime.InteropServices;

namespace NetCommander.Util
{
    public static class Win32
    {
    	public const uint SHGFI_ICON = 0x100;
    	public const uint SHGFI_LARGEICON = 0x0;
    	public const uint SHGFI_SMALLICON = 0x1;
    		
    	[DllImport("shell32.dll")]
    	public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
    }
}
