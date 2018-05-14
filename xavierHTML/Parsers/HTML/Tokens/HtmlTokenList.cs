using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parser;

namespace xavierHTML.Parsers.HTML.Tokens
{
    public class HtmlTokenList : ParseTree
    {
        private int currentLineIndex = -1;
        private SourceEnumerator lines;
        private LineEnumerator chars;
        private readonly List<Token> _tokens;

        public HtmlTokenList(Source source)
        {
            _tokens = Parse(source);
        }

        public override ParseTreeEnumerator GetEnumerator()
        {
            return new HtmlTokensEnumerator(_tokens);
        }

        private bool NextLine()
        {
            lines.MoveNext();
            currentLineIndex += 1;
            return !Eof;
        }

        private List<Token> Parse(Source source)
        {
            var tokens = new List<Token>();
            
            lines = source.GetEnumerator();
            NextLine();
            if (Eof)
            {
                return tokens;
            }
            chars = new LineEnumerator(lines.Current.ToString());
            ParseNode();

            return tokens;
        }
        
        private bool StartsWith(string s)
        {
            return chars.Content.Substring(chars.Position).StartsWith(s);
        }

        private char Consume()
        {
            var c = chars.Current;
            chars.MoveNext();
            if (Eol)
            {
                NextLine();
                chars = new LineEnumerator(lines.Current.ToString());
                chars.MoveNext();
            }
            return c;
        }

        private Token ConsumeToken(Token token)
        {
            foreach (var c in token.ToString())
            {
                if (Eof)
                {
                    throw new ParserException("Unexpected end of file", Position);
                }

                var current = chars.Current;
                if (current != c)
                {
                    throw new ParserException($"Expected '{c}' character, saw '{current}'", Position);
                }

                Consume();
            }

            return token;
        }

        private Token ConsumeQuoteToken()
        {
            try
            {
                if (chars.Current == ConstantTokens.SingleQuote[0])
                {
                    return ConsumeToken(ConstantTokens.SingleQuote);
                }

                if (chars.Current == ConstantTokens.DoubleQuote[0])
                {
                    return ConsumeToken(ConstantTokens.DoubleQuote);
                }
            }
            catch (ParserException)
            {
            }
            
            throw new ParserException("Expected a quote character", Position);
        }

        private string ConsumeWhile(Predicate<char> test)
        {
            var s = new StringBuilder();
            while (!Eof && test(chars.Current))
            {
                s.Append(Consume());
            }
            return s.ToString();
        }

        private void ConsumeWhitespace()
        {
            ConsumeWhile(char.IsWhiteSpace);
        }

        private string ParseIdentifier()
        {
            var identifier = ConsumeWhile(Identifier.IsIdentifier);
            if (identifier.Length == 0)
            {
                throw new ParserException("Expected identifier", Position);
            }

            return identifier;
        }

        private string ParseOpeningTag()
        {
            _tokens.Add(ConsumeToken(ConstantTokens.OpeningTagLhs));
            ConsumeWhitespace();
            var tagName = ParseIdentifier();
            _tokens.Add(new Tag(tagName));
            ConsumeWhitespace();
            while (chars.Current != ConstantTokens.TagRhs[0])
            {
                _tokens.Add(new Attribute(ParseIdentifier()));
                ConsumeWhitespace();
                _tokens.Add(ConstantTokens.Equals);
                ConsumeWhitespace();
                var openingQuote = ConsumeQuoteToken();
                _tokens.Add(ConstantTokens.DoubleQuote);
                _tokens.Add(new Text(ConsumeWhile(c => !StartsWith(openingQuote.ToString()))));
                ConsumeToken(openingQuote);
                _tokens.Add(ConstantTokens.DoubleQuote);
                ConsumeWhitespace();
            }
            _tokens.Add(ConsumeToken(ConstantTokens.TagRhs));

            return tagName;
        }

        private void ParseClosingTag(string tagName)
        {
            _tokens.Add(ConsumeToken(ConstantTokens.ClosingTagLhs));
            ConsumeWhitespace();
            if (StartsWith(tagName))
            {
                _tokens.Add(new ClosingTag(tagName));
            }
            else
            {
                throw new ParserException($"Expected closing tag for {tagName} element", Position);
            }

            ConsumeWhitespace();
            _tokens.Add(ConsumeToken(ConstantTokens.TagRhs));
        }

        private void TryParseTextNode()
        {
            var text = ConsumeWhile(c => !StartsWith(ConstantTokens.OpeningTagLhs.ToString()));
            if (text.Length > 0)
            {
                _tokens.Add(new Text(text));
            }
        }

        private void ParseNode()
        {
            // Sibling text node
            TryParseTextNode();

            if (StartsWith(ConstantTokens.ClosingTagLhs.ToString()))
            {
                return;
            }
            
            var tagName = ParseOpeningTag();

            // Children nodes
            while (!Eof || StartsWith(ConstantTokens.ClosingTagLhs.ToString()))
            {
                ParseNode();
            }
            
            ParseClosingTag(tagName);
        }

        private Position Position => new Position(currentLineIndex + 1, chars.Position);
        private bool Eol => chars.Position == chars.Length;
        private bool Eof => lines.Current == lines.Lines.Last() && Eol;
    }
    
    public class HtmlTokensEnumerator : ParseTreeEnumerator
    {
        public readonly List<Token> Tokens;
        private int _position = -1;

        public override Token Current => Tokens[_position];

        public HtmlTokensEnumerator(List<Token> tokens)
        {
            Tokens = tokens;
        }

        public override bool MoveNext()
        {
            _position += 1;
            return _position < Tokens.Count;
        }

        public override void Reset()
        {
            _position = -1;
        }
    }
}
