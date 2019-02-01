using System;
using System.Linq;

namespace xavierHTML.CSS.Values
{
    public struct Color
    {
        public static readonly Color Transparent = new Color(0, 0, 0, 0);
        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Blue = new Color(0, 0, 255);

        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        public readonly byte A;

        public Color(byte r, byte g, byte b) : this()
        {
            R = r;
            G = g;
            B = b;
            A = 255;
        }

        public Color(byte r, byte g, byte b, double a) : this()
        {
            R = r;
            G = g;
            B = b;
            A = (byte) (a.Clamp(0.0, 1.0) * 255.0).Round();
        }

        public Color(string hexColor) : this()
        {
            var bytes = Enumerable.Range(0, hexColor.Length / 2)
                .Select(x => Convert.ToByte(hexColor.Substring(x * 2, 2), 16)).ToArray();
            R = bytes[0];
            G = bytes[1];
            B = bytes[2];
            A = 255;
        }
    }
}
