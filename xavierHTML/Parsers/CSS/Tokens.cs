using System.Collections.Generic;
using Sprache;

namespace xavierHTML.Parsers.CSS
{
    public static class Tokens
    {
        public static Parser<IEnumerable<char>> Whitespace = Parse.WhiteSpace.Many();
        
        private static readonly Parser<char> _identifierStart =
            Sprache.Parse.Char(c => char.IsLetter(c) || c == '_', "letter, digit, or underscore");
        private static readonly Parser<char> _identifierChars =
            Sprache.Parse.Char(c => char.IsLetterOrDigit(c) || c == '_' || c == '-',
                "letter, digit, hyphen, or underscore");

        public static readonly Parser<string> Identifier =
            Sprache.Parse.Identifier(_identifierStart, _identifierChars);

        public static Parser<string> PrefixedIdentifier(char prefix) =>
            from p in Sprache.Parse.Char(prefix).Once()
            from id in Identifier
            select id;
    }
}
