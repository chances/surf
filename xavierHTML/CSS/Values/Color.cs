using System;
using System.Linq;

namespace xavierHTML.CSS.Values
{
    public struct Color
    {
        // https://www.w3.org/wiki/CSS/Properties/color/keywords
        public static readonly Color Transparent = new Color(0, 0, 0, 0);
        public static readonly Color Black = new Color("000000");
        public static readonly Color Silver = new Color("C0C0C0");
        public static readonly Color Gray = new Color("808080");
        public static readonly Color White = new Color("FFFFFF");
        public static readonly Color Maroon = new Color("800000");
        public static readonly Color Red = new Color("FF0000");
        public static readonly Color Purple = new Color("800080");
        public static readonly Color Fuchsia = new Color("FF00FF");
        public static readonly Color Green = new Color("008000");
        public static readonly Color Lime = new Color("00FF00");
        public static readonly Color Olive = new Color("808000");
        public static readonly Color Yellow = new Color("FFFF00");
        public static readonly Color Navy = new Color("000080");
        public static readonly Color Blue = new Color("0000FF");
        public static readonly Color Teal = new Color("008080");
        public static readonly Color Aqua = new Color("00FFFF");

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
