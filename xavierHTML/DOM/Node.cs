using System.Collections.Generic;

namespace xavierHTML.DOM
{
    public abstract class Node
    {
        public List<Node> Children { get; protected set; }
    }

    public class EmptyNode : Node
    {
        public EmptyNode()
        {
            Children = new List<Node>();
        }
    }
}
