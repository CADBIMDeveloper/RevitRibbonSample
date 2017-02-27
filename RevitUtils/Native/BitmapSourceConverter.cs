using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace RevitUtils.Native
{
    public class BitmapSourceConverter
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Преобразует <paramref name="image"/> в <see cref="BitmapSource"/>
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static BitmapSource ConvertFromImage(Bitmap image)
        {
            lock (image)
            {
                IntPtr hBitmap = image.GetHbitmap();
                try
                {
                    var bs = Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());

                    return bs;
                }
                finally
                {
                    DeleteObject(hBitmap);
                }
            }
        }

        /// <summary>
        /// Преобразует <paramref name="icon"/> в <see cref="BitmapSource"/>
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static BitmapSource ConvertFromIcon(Icon icon)
        {

            try
            {
                var bs = Imaging
                    .CreateBitmapSourceFromHIcon(icon.Handle,
                        new Int32Rect(0, 0, icon.Width, icon.Height),
                        BitmapSizeOptions.FromWidthAndHeight(icon.Width,
                            icon.Height));
                return bs;
            }
            finally
            {
                DeleteObject(icon.Handle);
                icon.Dispose();
                // ReSharper disable RedundantAssignment
                icon = null;
                // ReSharper restore RedundantAssignment
            }
        }
    }
}