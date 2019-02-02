using System.Drawing;
using Sprache;

namespace xavierHTML.CSS.Values
{
    public abstract class Value
    {
        public static readonly Parser<Value> Parser =
            Length.Parser
                .Or<Value>(String.Parser)
                .Or(Keyword.Parser)
                .Or(ColorValue.Parser)
                .Or(Function.Parser);

        public float ToPixels(float containerLength = 0.0f)
        {
            // Take identity of pixel length vales
            if (this is Length px && px.Unit == Unit.Pixels) return (float) px.Value;
            
            // Calculate percentage length values as percent of given container length
            if (this is Length percent && percent.Unit == Unit.Percentage)
                return (float) (percent.Value / 100.0 * containerLength);
            
            // TODO: Other maths to convert other Length unit values to pixels
            return 0.0f;
        }
    }
}
