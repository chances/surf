using System;
using Sprache;

namespace xavierHTML.CSS.Values
{
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

        public static new readonly Parser<Length> Parser =
            from value in _number
            from unit in Unit.Parse
            select new Length(value, unit);

        public override string ToString() => $"{Math.Round(Value, 5)}{Unit.Notation}";
    }
}
