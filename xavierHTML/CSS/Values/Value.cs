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

        public float ToPixels()
        {
            if (this is Length px && px.Unit == Unit.Pixels) return (float) px.Value;
            // TODO: Other maths to convert other Length unit values to pixels
            return 0.0f;
        }
    }
}
