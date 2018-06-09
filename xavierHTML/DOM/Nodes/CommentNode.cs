namespace xavierHTML.DOM
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
