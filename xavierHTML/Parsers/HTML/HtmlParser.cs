using System;
using System.Collections.Generic;
using System.Linq;
using Parser;
using xavierHTML.DOM;
using xavierHTML.Parsers.HTML.Tokens;
using Attribute = xavierHTML.Parsers.HTML.Tokens.Attribute;

namespace xavierHTML.Parsers.HTML
{
    public class HtmlParser
    {
        private readonly HtmlTokensEnumerator _tokens;

        public static Element EmptyDocument => new Element("html", new List<Node>()
        {
            new Element("body")
        });
        
        public HtmlParser(string input)
        {
            var tokenizer = new HtmlTokenizer(input);
            _tokens = tokenizer.Parse().GetEnumerator() as HtmlTokensEnumerator;
        }

        public Element Parse()
        {
            var rootNodes = new List<Node>();
            if (_tokens?.MoveNext() ?? false)
            {
                rootNodes.AddRange(ParseNodes());
            }

            Element document;
            if (rootNodes.Count == 0)
            {
                return EmptyDocument;
            }
            else if (rootNodes.Count == 1)
            {
                var rootNode = rootNodes[0];
                if (rootNode is Element element)
                {
                    var tagName = element.TagName.ToLower();
                    if (tagName.Equals("html"))
                    {
                        return element;
                    }
                    else if (tagName.Equals("head"))
                    {
                        document = EmptyDocument;
                        document.Children.Prepend(rootNode);
                        return document;
                    }
                    else if (tagName.Equals("body"))
                    {
                        document = EmptyDocument;
                        document.Children.Clear();
                        document.Children.Add(element);
                        return document;
                    }
                }
            }

            document = EmptyDocument;
            document.Children[0].Children.AddRange(rootNodes);
            return document;
        }

        private void ConsumeToken(Token token)
        {
            if (_tokens.Current != token)
            {
                throw new Exception($"Unexpected token {token}");
            }

            _tokens.MoveNext();
        }

        private string ConsumeTag(string expectedName = null)
        {
            var token = _tokens.Current;
            if (!(token is Tag))
            {
                throw new Exception($"Unexpected token {token}");
            }

            var tagName = (token as Tag).ToString();
            if (expectedName != null && !tagName.Equals(expectedName))
            {
                throw new Exception($"Expected closing tag for {tagName} element");
            }

            _tokens.MoveNext();
            
            return tagName;
        }
        
        private string ConsumeAttribute()
        {
            var token = _tokens.Current;
            if (!(token is Attribute))
            {
                throw new Exception($"Unexpected token {token}");
            }

            _tokens.MoveNext();
            
            return (token as Attribute).ToString();
        }
        
        private string ConsumeText()
        {
            var token = _tokens.Current;
            if (!(token is Text))
            {
                throw new Exception($"Unexpected token {token}");
            }

            _tokens.MoveNext();
            
            return (token as Text).ToString();
        }

        private List<Node> ParseNodes()
        {
            var nodes = new List<Node>();
            
            var token = _tokens.Current;
            while (!token.Equals(ConstantTokens.ClosingTagLhs))
            {
                if (token is Text text)
                {
                    var textContent = text.ToString().Trim("\t".ToCharArray());
                    nodes.Add(new TextNode(textContent));
                }
                else if (token.Equals(ConstantTokens.OpeningTagLhs))
                {
                    nodes.Add(ParseElement());
                }
                else
                {
                    throw new Exception($"Unexpected token {token}");
                }
            }

            return nodes;
        }

        private Element ParseElement()
        {
            ConsumeToken(ConstantTokens.OpeningTagLhs);
            var element = new Element(ConsumeTag());
            while (!_tokens.Current.Equals(ConstantTokens.TagRhs))
            {
                var attributeName = ConsumeAttribute();
                ConsumeToken(ConstantTokens.Equals);
                ConsumeToken(ConstantTokens.DoubleQuote);
                var attributeValue = ConsumeText();
                ConsumeToken(ConstantTokens.DoubleQuote);
                element.Attributes.Add(attributeName, attributeValue);
                
            }
            ConsumeToken(ConstantTokens.TagRhs);
            
            // Child nodes
            element.Children.AddRange(ParseNodes());
            
            // Closing tag
            ConsumeToken(ConstantTokens.ClosingTagLhs);
            ConsumeTag(element.TagName);
            ConsumeToken(ConstantTokens.TagRhs);

            return element;
        }
    }
}