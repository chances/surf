using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using xavierHTML.Parsers;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS.Values
{
    public abstract class Value
    {
        public static readonly Parser<Value> Parser = Length.Parser.Or<Value>(String.Parser).Or(Keyword.Parser)
            .Or(ColorValue.Parser).Or(Function.Parser);
    }

    public class Keyword : Value
    {
        public Keyword(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static readonly Parser<Keyword> Parser =
            from keyword in Tokens.Identifier
            select new Keyword(keyword);

        public override string ToString() => Value;
    }

    public class String : Value
    {
        public String(string value)
        {
            Value = value;
        }

        public string Value { get; }

        private static readonly Parser<char> _singleQuote = Sprache.Parse.Char('\'');
        private static readonly Parser<char> _doubleQuote = Sprache.Parse.Char('"');

        private static readonly char[] lineFeeds = {'\n','\r','\f'};
        private static readonly Parser<char> _singleQuoteStringBody = Sprache.Parse.Char(c =>
        {
            var notLineFeedsOrEscapedQuotes = !lineFeeds.Contains(c) && c != '\'';
            var notWhitespace = !char.IsWhiteSpace(c);
            return notLineFeedsOrEscapedQuotes && notWhitespace;
        }, "string body");
        private static readonly Parser<char> _doubleQuoteStringBody = Sprache.Parse.Char(c =>
        {
            var notLineFeedsOrEscapedQuotes = !lineFeeds.Contains(c) && c != '"';
            var notWhitespace = !char.IsWhiteSpace(c);
            return notLineFeedsOrEscapedQuotes && notWhitespace;
        }, "string body");

        private static readonly Parser<string> _singleQuoteString =
            from open in _singleQuote
            from body in _singleQuoteStringBody.Many().Text()
            from close in _singleQuote
            select body;
        private static readonly Parser<string> _doubleQuoteString =
            from open in _doubleQuote
            from body in _doubleQuoteStringBody.Many().Text()
            from close in _doubleQuote
            select body;

        public static readonly Parser<String> Parser = _singleQuoteString.Or(_doubleQuoteString)
            .Select(s => new String(s));
        
        public override string ToString() => Value;
    }

    public class Length : Value
    {
        public Length(double value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        public static Length Zero<T>() where T : Unit =>
            new Length(0.0, Activator.CreateInstance<T>());

        public double Value { get; }
        public Unit Unit { get; }

        private static readonly Parser<double> _number = Sprache.Parse.DecimalInvariant.Select(s =>
            !double.TryParse(s, out var number) ? double.NegativeInfinity : number);

        public static readonly Parser<Length> Parser =
            from value in _number
            from unit in Unit.Parse
            select new Length(value, unit);

        public override string ToString() => $"{Math.Round(Value, 5)}{Unit.Notation}";
    }

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

        public static readonly Parser<ColorValue> Parser =
            _3hexDigitsColor.Or(_6hexDigitsColor).Select(color => new ColorValue(color));

        public override string ToString()
        {
            var alpha = Math.Round(Color.a / 255.0, 2);
            return $"rgba({Color.r}, {Color.g}, {Color.b}, {alpha})";
        }
    }
    
    public static class DoubleExtensions
    {
        public static double Clamp (this double self, double min, double max)
        {
            return Math.Min (max, Math.Max (self, min));
        }

        public static int Round(this double self)
        {
            return (int) Math.Round(self, 0);
        }
    }

    public struct Color
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public Color(byte r, byte g, byte b) : this()
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 255;
        }

        public Color(byte r, byte g, byte b, double a) : this()
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = (byte) (a.Clamp(0.0, 1.0) * 255.0).Round();
        }

        public Color(string hexColor) : this()
        {
            var bytes = Enumerable.Range(0, hexColor.Length / 2)
                .Select(x => Convert.ToByte(hexColor.Substring(x * 2, 2), 16)).ToArray();
            this.r = bytes[0];
            this.g = bytes[1];
            this.b = bytes[2];
            this.a = 255;
        }
    }

    public class Function : Value
    {
        public Function(string name, List<Value> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }
        public List<Value> Parameters { get; }

        public static readonly Parser<Function> Parser =
            from name in Tokens.Identifier
            from open in Sprache.Parse.Char('(')
            from op_ws in Tokens.Whitespace
            from parameters in Value.Parser.DelimitedBy(Tokens.Whitespace)
            from close in Sprache.Parse.Char(')')
            from close_ws in Tokens.Whitespace
            select new Function(name, parameters.ToList());
    }
}
