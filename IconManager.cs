using System;
using System.Windows.Media.Imaging;

namespace Parameters
{
    public static class IconManager
    {
        public static BitmapSource getImg(IntPtr bitMapSource)
        {
            BitmapSource bms = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bitMapSource, IntPtr.Zero, System.Windows.Int32Rect.Empty,BitmapSizeOptions.FromEmptyOptions());
            return bms;
        }
    }
}
