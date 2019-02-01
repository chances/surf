using System.Collections.Generic;
using System.Linq;
using Sprache;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS.Values
{
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
