using System.Collections.Generic;
using System.Linq;
using Sprache;
using xavierHTML.CSS;
using xavierHTML.CSS.Selectors;

namespace xavierHTML.Parsers.CSS
{
    public class CssParser
    {
        public static Stylesheet Parse(string input)
        {
            var ruleset = _ruleset.Parse(input).ToList();
            return new Stylesheet(ruleset);
        }

        private static readonly Parser<IEnumerable<Rule>> _ruleset =
            Rule.Parse.DelimitedBy(Tokens.Whitespace);
    }
}
