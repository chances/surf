using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace xavierHTML.DOM.Elements
{
    public class Element : Node
    {
        public string Ns { get; } = "html";
        public string TagName { get; }
        public Dictionary<string, string> Attributes { get; }

        public Element(string tagName) : this(tagName, new List<Node>())
        {
        }

        public Element(string tagName, IEnumerable<Node> children) : this(tagName, new Dictionary<string, string>(), children)
        {
        }

        public Element(string tagName, Dictionary<string, string> attributes, IEnumerable<Node> children)
        {
            TagName = tagName;
            Attributes = attributes;
            foreach (var child in children)
            {
                Children.Add(child);
            }
        }

        public string Id => Attributes.FirstOrDefault(pair => pair.Key == "id").Value;
        public string ClassName => Attributes.FirstOrDefault(pair => pair.Key == "class").Value;

        public List<string> ClassList
        {
            get
            {
                var className = ClassName;
                if (className == null) return new List<string>();
                return className.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            }
        }

        public bool Matches(string[] selectors)
        {
            return false;
        }
    }
}
