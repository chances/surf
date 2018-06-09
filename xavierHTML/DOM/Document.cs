using System;
using System.Collections.Generic;
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
        private Element _document;

        private readonly Element _body;
        public Element Body => _body;

        private readonly Element _head;
        public Element Head => _head;

        public Document(Element document)
        {
            _document = document;
            _body = document.Children.OfType<Element>().FirstOrDefault(element => element.TagName == "body");
            _head = document.Children.OfType<Element>().FirstOrDefault(element => element.TagName == "head");
            
            // Parse inline stylesheets
            try
            {
                Stylesheets = _head.Children.Where(node => node is StyleNode)
                    .Select(node => CssParser.Parse(((StyleNode) node).Contents)).ToList();
            }
            catch (ParserException e)
            {
                Console.WriteLine(e);
                Stylesheets = new List<Stylesheet>();
            }
        }

        public Element DocumentElement => _document;

        public List<Stylesheet> Stylesheets { get; }
    }

    public enum DocumentReadyState
    {
        Loading,
        Interactive,
        Complete
    }
}