using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace ColorFlow
{
    public class Filters   
    {
        public static Func<Color, Color> Grayscale = input =>
        {
            int avg = (input.R + input.G + input.B) / 3;
            return Color.FromArgb(avg, avg, avg);
        };

        public static Func<Color, Color> Invert = input =>
        {
             return Color.FromArgb(255 - input.R, 255 - input.G, 255 - input.B);
        };

        public static Func<Color, Color> SepiaTone = input =>
        {
            Color nColor = Grayscale(input);
            byte outputRed = (byte)Math.Min(255,(nColor.R * 0.95) + 20);
            byte outputGreen = (byte)Math.Min(255, (nColor.G * 0.80) + 20);
            byte outputBlue = (byte)Math.Min(255, (nColor.R * 0.60) + 20);
            return Color.FromArgb(outputRed, outputGreen, outputBlue);
        };
        public static Func<Color, Color> sepia = input =>
        {
            int green = input.G;
            int blue = input.B;
            int red = input.R;
            byte outputRed = (byte)Math.Min(255, (red * .7) + (green * .769) + (blue * .189));
            byte outputGreen = (byte)Math.Min(255, (red * .349) + (green * .686) + (blue * .168));
            byte outputBlue = (byte)Math.Min(255, (red * .272) + (green * .534) + (blue * .131));
            return Color.FromArgb(outputRed, outputGreen, outputBlue);
        };

        public static Func<Color, Color> Darkblue = input =>
        {
            var output = Invert(input);
            output = sepia(output);
            return Invert(output);
        };

    }
}