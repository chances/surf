using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS.Values;

namespace xavierHTML.CSS.Properties
{
    public static class Padding
    {
        public static EdgeValues GetPaddings(Dictionary<string, List<Value>> specifiedValues)
        {
            var properties = specifiedValues.Keys.ToList();

            var paddings = new EdgeValues(new List<Value>(0));

            // Assign from specified shorthand property
            var paddingShorthand = Property.GetValues(specifiedValues, "padding");
            if (paddingShorthand != null && paddingShorthand.TrueForAll(v => v is Length))
                paddings = new EdgeValues(paddingShorthand);

            // Overwrite with any subsequently specified specific paddings
            var paddingTop = Property.GetValue(specifiedValues, "padding-top");
            if (paddingTop != null && properties.IndexOf("padding-top") > properties.IndexOf("padding"))
            {
                paddings.Top = paddingTop;
            }

            var paddingRight = Property.GetValue(specifiedValues, "padding-right");
            if (paddingRight != null && properties.IndexOf("padding-right") > properties.IndexOf("padding"))
            {
                paddings.Right = paddingRight;
            }

            var paddingBottom = Property.GetValue(specifiedValues, "padding-bottom");
            if (paddingBottom != null && properties.IndexOf("padding-bottom") > properties.IndexOf("padding"))
            {
                paddings.Bottom = paddingBottom;
            }

            var paddingLeft = Property.GetValue(specifiedValues, "padding-left");
            if (paddingLeft != null && properties.IndexOf("padding-left") > properties.IndexOf("padding"))
            {
                paddings.Left = paddingLeft;
            }

            return paddings;
        }
    }
}
