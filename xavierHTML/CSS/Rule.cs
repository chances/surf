using System.Collections.Generic;
using System.Linq;
using Sprache;
using xavierHTML.CSS.Selectors;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS
{
    public class Rule
    {
        public Rule(List<Selector> selectors, List<Declaration> declarations)
        {
            Selectors = selectors;
            Declarations = declarations;
        }

        public List<Selector> Selectors { get; }
        public List<Declaration> Declarations { get; }
        
        private static readonly Parser<IEnumerable<Selector>> _selectors =
        (
            from _ in Tokens.Whitespace
            from selector in SimpleSelector.Parse
            select selector
        ).DelimitedBy(Sprache.Parse.Char(','));

        private static readonly Parser<IEnumerable<Declaration>> _declarations =
            Declaration.Parser.DelimitedBy(Tokens.Whitespace);

        public static readonly Parser<Rule> Parse =
            from selectors in _selectors
            from selectors_ws in Tokens.Whitespace
            from open in Sprache.Parse.Char('{')
            from open_ws in Tokens.Whitespace
            from declarations in _declarations
            from decl_ws in Tokens.Whitespace
            from close in Sprache.Parse.Char('}').Named("closing brace")
            select new Rule(selectors.ToList(), declarations.ToList());
    }
}
