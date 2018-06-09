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

        // TODO: Implement inner text algorithm https://html.spec.whatwg.org/multipage/dom.html#dom-innertext
        public string InnerText => TextContent;

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

        public IEnumerable<Element> GetElementsByTagName(string qualifiedName)
        {
            var children = Children.OfType<Element>().ToList();
            
            if (qualifiedName == "*")
            {
                return children;
            }

            var matchingElements = children.Where(e => e.TagName == qualifiedName);
            var matchingChildren = children.SelectMany(element => element.GetElementsByTagName(qualifiedName));
            return matchingElements.Concat(matchingChildren);
        }
    }
}
