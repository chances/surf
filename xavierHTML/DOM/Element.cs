using System.Collections.Generic;

namespace xavierHTML.DOM
{
    public class Element : Node
    {
        public string Ns { get; } = "html";
        public string TagName { get; }
        public Dictionary<string, string> Attributes { get; }
        
        public Element(string tagName) : this(tagName, new List<Node>())
        {}
        
        public Element(string tagName, List<Node> children) : this(tagName, new Dictionary<string, string>(), children)
        {}

        public Element(string name, Dictionary<string, string> attributes, List<Node> children)
        {
            TagName = name;
            Attributes = attributes;
            Children = children;
        }
    }
}
