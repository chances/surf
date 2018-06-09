using System;
using xavierHTML.CSS.Selectors;

namespace xavierHTML.CSS
{
    public class Specificity : IComparable<Specificity>
    {
        public Specificity(Selector selector)
        {
            TagParts = 0;
            IdParts = 0;
            ClassParts = 0;

            switch (selector)
            {
                case SimpleSelector s:
                    TagParts = (ushort) (s.TagName == null ? 0 : 1);
                    IdParts = (ushort) (s.Id == null ? 0 : 1);
                    ClassParts = (ushort) s.Classes.Length;
                    break;
            }
        }

        public ushort TagParts { get; }
        public ushort IdParts { get; }
        public ushort ClassParts { get; }

        public int CompareTo(Specificity other)
        {
            var sum = TagParts + IdParts + ClassParts;
            var otherSum = other.TagParts + other.IdParts + other.ClassParts;
            return sum.CompareTo(otherSum);
        }
    }
}
