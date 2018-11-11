﻿using System;
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
        public Element DocumentElement { get; }

        public Element Body { get; }
        public Element Head { get; }

        public List<Stylesheet> Stylesheets { get; }

        public Document(Element document)
        {
            DocumentElement = document;
            Body = document.GetElementsByTagName("body").LastOrDefault();
            Head = document.GetElementsByTagName("head").FirstOrDefault();
            
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