using System;
using System.Linq;
using Sprachtml.Models;
using xavierHTML.DOM;
using ScriptNode = Sprachtml.Models.ScriptNode;
using StyleNode = Sprachtml.Models.StyleNode;
using TextNode = Sprachtml.Models.TextNode;

namespace xavierHTML.Parsers.HTML
{
    public static class HtmlNodeExtensions
    {
        public static Element ToElement(this HtmlNode node)
        {
            var attributes = node.Attributes.Select(attribute => Tuple.Create(attribute.Name, attribute.Value?.Text))
                .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            var children = node.Children.Select(n => n.ToNode()).Where(i => i != null).ToList();

            return new Element(node.TagName, attributes, children);
        }

        public static Node ToNode(this IHtmlNode node)
        {
            switch (node)
            {
                case HtmlNode n:
                    return n.ToElement();
                case TextNode n:
                    return new DOM.TextNode(n.Contents);
                case StyleNode n:
                    return new DOM.StyleNode(n.Contents);
                case ScriptNode n:
                    return new DOM.ScriptNode(n.Contents);
                default:
                    return null;
            }
        }
    }
}
