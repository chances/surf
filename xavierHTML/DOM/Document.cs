using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using xavierHTML.CSS;
using xavierHTML.DOM.Elements;
using xavierHTML.Parsers;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.DOM
{
    public class Document
    {
        private string domain;
        private string referrer;
        private string lastModified;
        private DocumentReadyState readyState = DocumentReadyState.Loading;

        private string title;
        public Element DocumentElement { get; }

        public Element Body { get; }
        public Element Head { get; private set; }

        public string Title
        {
            get => title;
            set
            {
                var titleElement = Head?.GetElementsByTagName("title").FirstOrDefault();
                if (titleElement == null)
                {
                    if (Head == null)
                    {
                        Head = new Element("head");
                        DocumentElement.Children.Add(Head);
                    }
                    titleElement = new Element("title", new []{new TextNode(value)});
                    Debug.Assert(Head != null, nameof(Head) + " != null");
                    Head.Children.Add(titleElement);
                }
                else
                {
                    titleElement.Children.Clear();
                    titleElement.Children.Add(new TextNode(value));
                }
            }
        }

        public List<Stylesheet> Stylesheets { get; }

        public Document(Element document)
        {
            DocumentElement = document;
            Body = document.GetElementsByTagName("body").LastOrDefault();
            Head = document.GetElementsByTagName("head").FirstOrDefault();

            var titleElement = Head?.GetElementsByTagName("title").FirstOrDefault();
            title = titleElement?.TextContent;

            // Parse inline stylesheets
            try
            {
                Stylesheets = Head?.Children.Where(node => node is StyleNode)
                    .Select(node => CssParser.Parse(((StyleNode) node).Contents)).ToList();
            }
            catch (ParserException e)
            {
                Console.WriteLine(e);
                Stylesheets = new List<Stylesheet>();
            }
        }
    }

    public enum DocumentReadyState
    {
        Loading,
        Interactive,
        Complete
    }
}