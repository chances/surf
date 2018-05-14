using System;

namespace xavierHTML.Parsers
{
    public class ParserException : Exception
    {
        public Position Position { get; private set; }

        public ParserException(string message, Position position) : base(message)
        {
            Position = position;
        }

        public ParserException(string message, Exception innerException, Position position) : base(message, innerException)
        {
            Position = position;
        }
    }

    public struct Position
    {
        public int Line;
        public int Column;

        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }
}
