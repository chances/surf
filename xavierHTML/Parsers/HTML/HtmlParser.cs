using System;
using System.Collections.Generic;
using System.Linq;
using Sprachtml;
using Sprachtml.Exceptions;
using Sprachtml.Models;
using xavierHTML.DOM;

namespace xavierHTML.Parsers.HTML
{
    public class HtmlParser
    {
        public static Element EmptyDocument => new Element("html", new List<Node>()
        {
            new Element("body")
        });

        public static Element Parse(string input)
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

            if (rootNodes.Length >= 1 && rootNodes[0] is HtmlNode)
            {
                var rootNode = rootNodes[0];
                if (rootNode is HtmlNode element)
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
                else
                {
                    return EmptyDocument;
                }
            }

            document = EmptyDocument;
            document.Children[0].Children.AddRange(rootNodes.Select(node => node.ToNode()));
            return document;
        }
    }
}