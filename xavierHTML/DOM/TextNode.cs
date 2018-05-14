using System.Collections.Generic;

namespace xavierHTML.DOM
{
    public class TextNode : EmptyNode
    {
        public string Data { get; }

        public TextNode(string data)
        {
            Data = data;
        }
    }
}
