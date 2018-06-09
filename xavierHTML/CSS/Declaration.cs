using System.Collections.Generic;
using System.Linq;
using Sprache;
using xavierHTML.CSS.Values;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS
{
    public class Declaration
    {
        public Declaration(string name, List<Value> values, bool important = false)
        {
            Name = name;
            Values = values;
            Important = important;
        }

        public string Name { get; }
        public List<Value> Values { get; }
        public bool Important { get; }

        public static Parser<Declaration> Parser =
            from name in Tokens.Identifier
            from name_ws in Tokens.Whitespace
            from colon in Parse.Char(':')
            from colon_ws in Tokens.Whitespace
            from values in Value.Parser.DelimitedBy(Tokens.Whitespace)
            from value_ws in Tokens.Whitespace.Optional()
            from important in Parse.String("!important").Optional()
            from important_ws in Tokens.Whitespace
            from semicolon in Parse.Char(';')
            select new Declaration(name, values.ToList(), important.IsDefined);

        public override string ToString()
        {
            var values = Values.Aggregate("", (s, value) => $"{s} {value}");
            return $"{Name}:{values}";
        }
    }
}
