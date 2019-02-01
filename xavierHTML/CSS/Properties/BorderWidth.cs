using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS.Values;

namespace xavierHTML.CSS.Properties
{
    public static class BorderWidth
    {
        public static EdgeValues GetBorderWidths(Dictionary<string, List<Value>> specifiedValues)
        {
            var properties = specifiedValues.Keys.ToList();

            var borderWidths = new EdgeValues(new List<Value>(0));

            // Assign from specified shorthand property
            var borderWidthShorthand = Property.GetValues(specifiedValues, "border-width");
            if (borderWidthShorthand != null && borderWidthShorthand.TrueForAll(v => v is Length))
                borderWidths = new EdgeValues(borderWidthShorthand);

            // Overwrite with any subsequently specified specific borderWidths
            var borderWidthTop = Property.GetValue(specifiedValues, "border-top-width");
            if (borderWidthTop != null && properties.IndexOf("border-top-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Top = borderWidthTop;
            }

            var borderWidthRight = Property.GetValue(specifiedValues, "border-right-width");
            if (borderWidthRight != null && properties.IndexOf("border-right-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Right = borderWidthRight;
            }

            var borderWidthBottom = Property.GetValue(specifiedValues, "border-bottom-width");
            if (borderWidthBottom != null && properties.IndexOf("border-bottom-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Bottom = borderWidthBottom;
            }

            var borderWidthLeft = Property.GetValue(specifiedValues, "border-left-width");
            if (borderWidthLeft != null && properties.IndexOf("border-left-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Left = borderWidthLeft;
            }

            return borderWidths;
        }
    }
}
