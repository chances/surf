namespace xavierHTML.DOM.Nodes
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
