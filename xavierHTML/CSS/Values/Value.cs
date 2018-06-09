using Sprache;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS.Values
{
    public abstract class Value
    {
        public static readonly Parser<Value> Parse = Length.Parser.Or<Value>(Keyword.Parser);
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
    }

    public class Length : Value
    {
        public Length(double value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        public double Value { get; }
        public Unit Unit { get; }

        private static readonly Parser<double> _number = Sprache.Parse.DecimalInvariant.Select(s =>
            !double.TryParse(s, out var number) ? double.NegativeInfinity : number);

        public static readonly Parser<Length> Parser =
            from value in _number
            from unit in Unit.Parse
            select new Length(value, unit);
    }

    public class ColorValue : Value
    {
        public ColorValue(Color color)
        {
            Color = color;
        }

        public Color Color { get; }
    }

    public struct Color
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
    }
}
