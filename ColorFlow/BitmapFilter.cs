using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ColorFlow
{
    public class BitmapFilter
    {

        public BitmapFilter() {  }
        public int Height { get; set; }
        public int Width { get; set; }
        public Byte[] Image { get; set; }


        public BitmapFilter(byte[] image)
        {
            this.Image = image;
            Bitmap bmp = image.ToBitmap();
            this.Width = bmp.Width;
            this.Height = bmp.Height;
        }

        public Byte[] Grayscale() { return BasicFiltrationUsing(Filters.Grayscale).ToBytes(); }
        public Byte[] Invert() { return BasicFiltrationUsing(Filters.Invert).ToBytes(); }
        public Byte[] SepiaTone() { return BasicFiltrationUsing(Filters.SepiaTone).ToBytes(); }
        public Byte[] DarkBlue() { return BasicFiltrationUsing(Filters.Darkblue).ToBytes(); }


        public unsafe Byte[] Mirror()
        {
            Bitmap bmp = this.Image.ToBitmap();
            Rectangle bmpRec = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(bmpRec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* pixelPointer = (byte*)bmpData.Scan0;
            int linePadding = bmpData.Stride - bmp.Width * 3;
            int pixelPadding = 3;

            // Filling up a Color[,] (matrix) to map colors
            // in an OO-list
            Color[,] matrix = new Color[bmp.Height, bmp.Width];
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    matrix[y, x] = Color.FromArgb((byte)pixelPointer[2], (byte)pixelPointer[1], (byte)pixelPointer[0]);
                    pixelPointer += pixelPadding;
                }
                pixelPointer += linePadding;
            }

            // going througt the pixel array AGAIN, and modifying it!
            pixelPointer = (byte*)bmpData.Scan0;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color input = matrix[y, (bmp.Width - 1) - x];
                    pixelPointer[2] = input.R;
                    pixelPointer[1] = input.G;
                    pixelPointer[0] = input.B;
                    pixelPointer += pixelPadding;
                }
                pixelPointer += linePadding;
            }
            bmp.UnlockBits(bmpData);
            return bmp.ToBytes();
        }



        private unsafe Bitmap BasicFiltrationUsing(Func<Color, Color> Filter)
        {
            Bitmap bmp = this.Image.ToBitmap();
            Rectangle bmpRec = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(bmpRec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* pixPointer = (byte*)bmpData.Scan0;
            int linePadding = bmpData.Stride - bmp.Width * 3;
            int pixelPadding = 3;
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color input = Color.FromArgb((byte)pixPointer[2], (byte)pixPointer[1], (byte)pixPointer[0]);
                    Color output = Filter(input);
                    pixPointer[2] = output.R;
                    pixPointer[1] = output.G;
                    pixPointer[0] = output.B;

                    pixPointer += pixelPadding;
                }
                pixPointer += linePadding;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        
    }
}