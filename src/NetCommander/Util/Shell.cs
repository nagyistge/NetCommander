using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace NetCommander.Util
{
    public static class Shell
    {
        public static ImageSource GetSmallIcon(string fileName)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgSmall = Win32.SHGetFileInfo(fileName, 0, ref shinfo,(uint)Marshal.SizeOf(shinfo),Win32.SHGFI_ICON |Win32.SHGFI_SMALLICON);
            Icon icon = Icon.FromHandle(shinfo.hIcon);
            return Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle, 
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
        
        public static ImageSource GetLargeIcon(string fileName)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgLarge = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            Icon icon = Icon.FromHandle(shinfo.hIcon);
            return Imaging.CreateBitmapSourceFromHIcon(
                        icon.Handle, 
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
