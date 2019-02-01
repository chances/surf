using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS.Values;

namespace xavierHTML.CSS.Properties
{
    public static class Margin
    {
        public static EdgeValues GetMargins(Dictionary<string, List<Value>> specifiedValues)
        {
            var properties = specifiedValues.Keys.ToList();

            var margins = new EdgeValues(new List<Value>(0));

            // Assign from specified shorthand property
            var marginShorthand = Property.GetValues(specifiedValues, "margin");
            if (marginShorthand != null && marginShorthand.TrueForAll(v => v is Length))
                margins = new EdgeValues(marginShorthand);

            // Overwrite with any subsequently specified specific margins
            var marginTop = Property.GetValue(specifiedValues, "margin-top");
            if (marginTop != null && properties.IndexOf("margin-top") > properties.IndexOf("margin"))
            {
                margins.Top = marginTop;
            }

            var marginRight = Property.GetValue(specifiedValues, "margin-right");
            if (marginRight != null && properties.IndexOf("margin-right") > properties.IndexOf("margin"))
            {
                margins.Right = marginRight;
            }

            var marginBottom = Property.GetValue(specifiedValues, "margin-bottom");
            if (marginBottom != null && properties.IndexOf("margin-bottom") > properties.IndexOf("margin"))
            {
                margins.Bottom = marginBottom;
            }

            var marginLeft = Property.GetValue(specifiedValues, "margin-left");
            if (marginLeft != null && properties.IndexOf("margin-left") > properties.IndexOf("margin"))
            {
                margins.Left = marginLeft;
            }

            return margins;
        }
    }
}
