using System;
using System.Text;

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

        public override string ToString()
        {
            var builder = new StringBuilder("at ");
            builder.Append(Position.Line);
            builder.Append(":");
            builder.Append(Position.Column);
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append(Message);

            if (InnerException == null) return builder.ToString();

            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            builder.Append(InnerException.Message);

            return builder.ToString();
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
