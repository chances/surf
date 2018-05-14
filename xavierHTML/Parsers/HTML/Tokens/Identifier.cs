using Parser;

namespace xavierHTML.Parsers.HTML.Tokens
{
    public class Identifier : Token
    {
        protected Identifier(string value) : base(value)
        {
        }
        
        public static bool IsIdentifier(char c)
        {
            return Utils.IsLatinLetter(c) || Utils.IsArabicNumeral(c);
        }
    }
}
