namespace xavierHTML.DOM.Nodes
{
    public class CommentNode : EmptyNode
    {
        public string Comment { get; }

        public CommentNode(string comment)
        {
            Comment = comment;
        }
    }
}
