using System;
using System.Collections.Generic;
using System.Linq;
using Sprachtml;
using Sprachtml.Exceptions;
using Sprachtml.Models;
using xavierHTML.DOM;
using xavierHTML.DOM.Elements;
using xavierHTML.DOM.Nodes;

namespace xavierHTML.Parsers.HTML
{
    public class HtmlParser
    {
        public static Element EmptyDocument => new Element("html", new List<Node>()
        {
            new Element("body")
        });

        public static Document Parse(string input)
        {
            return new Document(ParseDocumentElement(input));
        }

        private static Element ParseDocumentElement(string input)
        {
            IHtmlNode[] rootNodes;
            try
            {
                rootNodes = SprachtmlParser.Parse(input);
            }
            catch (SprachtmlParseException e)
            {
                Console.WriteLine(e);
                throw new ParserException(e.Message, e, new Position(e.NodeLocation.Line, e.NodeLocation.Column));
            }

            Element document;
            if (rootNodes.Length == 0)
            {
                return EmptyDocument;
            }

            if (rootNodes.Length == 1 && rootNodes[0] is DocTypeNode docType)
            {
                // TODO: Handle legacy doc types?
                return EmptyDocument;
            }

            if (rootNodes.Length >= 1 && rootNodes[0] is HtmlNode element)
            {
                var tagName = element.TagName.ToLower();
                if (tagName.Equals("html"))
                {
                    return element.ToElement();
                }
                else if (tagName.Equals("head"))
                {
                    document = EmptyDocument;
                    document.Children.Prepend(element.ToElement());
                    return document;
                }
                else if (tagName.Equals("body"))
                {
                    document = EmptyDocument;
                    document.Children.Clear();
                    document.Children.Add(element.ToElement());
                    return document;
                }
            }

            document = EmptyDocument;
            foreach (var n in rootNodes.Select(node => node.ToNode()))
            {
                document.Children[0].Children.Add(n);
            }
            return document;
        }
    }
}