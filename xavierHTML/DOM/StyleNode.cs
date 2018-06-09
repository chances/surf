namespace xavierHTML.DOM
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
