using System;
using Sprache;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS.Values
{
    public class Keyword : Value
    {
        public Keyword(string value)
        {
            Value = value;
        }
        
        public static Keyword Auto = new Keyword("auto");

        public string Value { get; }

        public static readonly Parser<Keyword> Parser =
            from keyword in Tokens.Identifier
            select new Keyword(keyword);

        public override string ToString() => Value;

        protected bool Equals(Keyword other)
        {
            return string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Keyword other && Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode(Value);
        }

        public static bool operator ==(Keyword left, Keyword right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Keyword left, Keyword right)
        {
            return !Equals(left, right);
        }
    }
}
