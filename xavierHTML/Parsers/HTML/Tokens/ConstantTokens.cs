using Parser;

namespace xavierHTML.Parsers.HTML.Tokens
{
    public class ConstantTokens
    {
        public static readonly Token OpeningTagLhs = new Token("<");
        public static readonly Token ClosingTagLhs = new Token("</");
        public static readonly Token TagRhs = new Token(">");
        
        public new static readonly Token Equals = new Token("=");
        public static readonly Token DoubleQuote = new Token("\"");
        public static readonly Token SingleQuote = new Token("'");
        
        public static readonly Token DoctypeLhs = new Token("<!DOCTYPE");
        public static readonly Token DoctypeRhs = TagRhs;
    }
}
