using System.Collections.Generic;
using System.Linq;

namespace xavierHTML.CSS.Values
{
    public struct EdgeValues
    {
        /// <summary>
        /// Parse a set of edge values (i.e. margin, padding) from a given list of ordered shorthand values.
        /// </summary>
        /// <remarks>
        /// Implements the CSS 2 Shorthand Values spec: http://www.w3.org/TR/CSS2/about.html#shorthand
        /// </remarks>
        /// <param name="shorthandValues">List of ordered shorthand values.</param>
        public EdgeValues(IReadOnlyList<Value> shorthandValues)
        {
            switch (shorthandValues.Count)
            {
                case 1:
                    var value = shorthandValues.First();

                    Top = value;
                    Right = value;
                    Bottom = value;
                    Left = value;
                    break;
                case 2:
                    var first = shorthandValues.First();
                    var last = shorthandValues.Last();

                    Top = first;
                    Right = last;
                    Bottom = first;
                    Left = last;
                    break;
                case 3:
                    Top = shorthandValues.First();
                    Right = shorthandValues[1];
                    Bottom = shorthandValues.Last();
                    Left = shorthandValues[1];
                    break;
                case 4:
                    Top = shorthandValues[0];
                    Right = shorthandValues[1];
                    Bottom = shorthandValues[2];
                    Left = shorthandValues[3];
                    break;
                default:
                    var zero = Length.Zero<Pixels>();
            
                    Top = zero;
                    Right = zero;
                    Bottom = zero;
                    Left = zero;
                    break;
            }
        }

        public EdgeValues(Value top, Value right, Value bottom, Value left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public Value Top;
        public Value Right;
        public Value Bottom;
        public Value Left;
    }
}
