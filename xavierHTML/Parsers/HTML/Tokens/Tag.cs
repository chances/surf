using System.Collections.Generic;
using Parser;

namespace xavierHTML.Parsers.HTML.Tokens
{
    public sealed class Tag : Identifier
    {
        public Tag(string name) : base(name)
        {
        }
    }
    
    public sealed class ClosingTag : Identifier
    {
        public ClosingTag(string name) : base(name)
        {
        }
    }
}
