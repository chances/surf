using System;
using Sprache;
using xavierHTML.Parsers;

namespace xavierHTML.CSS.Values
{
    public class ColorValue : Value
    {
        public ColorValue(Color color)
        {
            Color = color;
        }

        public Color Color { get; }

        private static readonly Parser<char> _hexDigit = Parse.Char(Utils.IsHexDigit, "hex digit");

        private static readonly Parser<Color> _3hexDigitsColor =
            from hash in Parse.Char('#')
            from r in _hexDigit
            from g in _hexDigit
            from b in _hexDigit
            select new Color(Convert.ToByte(r), Convert.ToByte(g), Convert.ToByte(b));

        private static readonly Parser<Color> _6hexDigitsColor =
            from hash in Parse.Char('#')
            from hex in Parse.Repeat(_hexDigit, 6).Text()
            select new Color(hex);

        public static new readonly Parser<ColorValue> Parser =
            _3hexDigitsColor.Or(_6hexDigitsColor).Select(color => new ColorValue(color));

        public override string ToString()
        {
            var alpha = Math.Round(Color.A / 255.0, 2);
            return $"rgba({Color.R}, {Color.G}, {Color.B}, {alpha})";
        }
    }
}
