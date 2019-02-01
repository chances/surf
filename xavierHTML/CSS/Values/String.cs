using System.Linq;
using Sprache;

namespace xavierHTML.CSS.Values
{
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
}
