using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace xavierHTML.Parsers.HTML
{
    public class LineEnumerator : IEnumerator
    {
        private readonly StringInfo _info;
        
        public int Position { get; private set; }
        public readonly string Content;
        
        public LineEnumerator(string content)
        {
            Content = content;
            _info = new StringInfo(Content);
            
        }

        public bool MoveNext()
        {
            ++Position;
            return Position < Length;
        }

        public void Reset()
        {
            Position = -1;
        }

        object IEnumerator.Current => Current;

        public char Current
        {
            get
            {
                try
                {
                    return _info.SubstringByTextElements(Position, 1)[0];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public int Length => _info.LengthInTextElements;
    }
}
