using System.Linq;
using Parser;
using xavierHTML.Parsers.HTML.Tokens;

namespace xavierHTML.Parsers.HTML
{
    public class StringSource : Source
    {
        protected internal StringSource(string input) : base(input.GetLines().Select((s, i) => new Line(i, s)).ToList())
        {}
    }
    
    public class HtmlTokenizer : Parser.Parser
    {
        public string Input { get; }

        public HtmlTokenizer(string input)
        {
            Input = input;
            Source = new StringSource(input);
        }

        public override ParseTree Parse()
        {
            return new HtmlTokenList(Source as StringSource);
        }
    }
}
