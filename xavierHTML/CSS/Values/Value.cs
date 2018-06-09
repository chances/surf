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
            .Or(Function.Parser);
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
    }

    public struct Color
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

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
