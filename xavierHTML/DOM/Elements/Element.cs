using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Sprache;
using xavierHTML.CSS.Selectors;
using xavierHTML.Parsers;

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
            
            // Parse style attributes as CSS
            if (Attributes.ContainsKey("style"))
            {
                var style = Attributes["style"];
                
            }
        }

        public string GetAttribute(string name)
        {
            return Attributes.ContainsKey(name) ? Attributes[name] : null;
        }

        public string Id => GetAttribute("id");
        public string ClassName => GetAttribute("class");

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

        public bool Matches(IEnumerable<string> selectors)
        {
            var parsedSelectors = new List<Selector>();
            foreach (var selector in selectors)
            {
                try
                {
                    parsedSelectors.Add(Selector.Parser.Parse(selector));
                }
                catch (ParserException e)
                {}
            }

            return parsedSelectors.Select(s => Matches(s))
                .Aggregate(false, (hasMatched, sMatches) => hasMatched || sMatches);
        }

        public bool Matches(Selector selector)
        {
            switch (selector)
            {
                case SimpleSelector simple:
                    var tagMatch = simple.TagName == null || simple.TagName == TagName;
                    var idMatch = simple.Id == null || simple.Id == Id;
                    var classList = ClassList;
                    var allSelectedClassesMatch = simple.Classes.All(s => classList.Contains(s));
                    return tagMatch && idMatch && allSelectedClassesMatch;
                    break;
                default:
                    return false;
            }
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
