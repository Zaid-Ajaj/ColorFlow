using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace ColorFlow
{
    public static class Extensions
    {
        public static byte[] ToBytes(this Bitmap bmp)
        {
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Bmp);
            return ms.ToArray();
        }

        public static Bitmap ToBitmap(this byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            Bitmap bmp = new Bitmap(ms);
            return bmp;
        }
    }
}