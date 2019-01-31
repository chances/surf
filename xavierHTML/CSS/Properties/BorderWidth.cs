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
            var borderWidthShorthand = specifiedValues["border-width"];
            if (borderWidthShorthand != null && borderWidthShorthand.TrueForAll(v => v is Length))
                borderWidths = new EdgeValues(borderWidthShorthand);

            // Overwrite with any subsequently specified specific borderWidths
            var borderWidthTop = specifiedValues["border-top-width"]?.FirstOrDefault();
            if (borderWidthTop != null && properties.IndexOf("border-top-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Top = borderWidthTop;
            }

            var borderWidthRight = specifiedValues["border-right-width"]?.FirstOrDefault();
            if (borderWidthRight != null && properties.IndexOf("border-right-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Right = borderWidthRight;
            }

            var borderWidthBottom = specifiedValues["border-bottom-width"]?.FirstOrDefault();
            if (borderWidthBottom != null && properties.IndexOf("border-bottom-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Bottom = borderWidthBottom;
            }

            var borderWidthLeft = specifiedValues["border-left-width"]?.FirstOrDefault();
            if (borderWidthLeft != null && properties.IndexOf("border-left-width") > properties.IndexOf("border-width"))
            {
                borderWidths.Left = borderWidthLeft;
            }

            return borderWidths;
        }
    }
}
