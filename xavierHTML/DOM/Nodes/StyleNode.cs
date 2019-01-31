namespace xavierHTML.DOM.Nodes
{
    public class StyleNode : EmptyNode
    {
        public string Contents { get; }

        public StyleNode(string contents)
        {
            Contents = contents;
        }
    }
}
