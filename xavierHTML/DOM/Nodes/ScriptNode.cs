namespace xavierHTML.DOM.Nodes
{
    public class ScriptNode : EmptyNode
    {
        public string Contents { get; }

        public ScriptNode(string contents)
        {
            Contents = contents;
        }
    }
}
