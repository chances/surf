using System.Collections.Generic;
using Sprache;
using xavierHTML.CSS.Values;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS
{
    public class Declaration
    {
        public Declaration(string name, Value value, bool important = false)
        {
            Name = name;
            Value = value;
            Important = important;
        }

        public string Name { get; }
        public Value Value { get; }
        public bool Important { get; }

        public static Parser<Declaration> Parser =
            from name in Tokens.Identifier
            from name_ws in Tokens.Whitespace
            from colon in Parse.Char(':')
            from colon_ws in Tokens.Whitespace
            from value in Value.Parse
            from value_ws in Tokens.Whitespace
            from important in Parse.String("!important").Optional()
            from important_ws in Tokens.Whitespace
            from semicolon in Parse.Char(';')
            select new Declaration(name, value, important.IsDefined);
    }
}
