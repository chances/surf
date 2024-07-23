using System;
#if Windows
using Drawing = System.Drawing;
#endif

using xavierHTML.Layout;
using xavierHTML.Layout.BoxModel;

namespace Surf.Rasterization
{
    public abstract class DisplayCommand
    {
        protected DisplayCommand(Box box)
        {
            Bounds = box.Dimensions.MarginBox;
            OpaqueBounds = box.Dimensions.BorderBox;
        }

        public Rectangle Bounds { get; }
        public Rectangle OpaqueBounds { get; }
    }

    public class SolidColor : DisplayCommand
    {
        public SolidColor(NodeBox box) : base(box)
        {
#if Windows
            Color = box.Style.BackgroundColor.ToColor();
            BorderColor = box.Style.BorderColor.ToColor();
#elif Linux
            Color = box.Style.BackgroundColor.ToGdkColor();
            BorderColor = box.Style.BorderColor.ToGdkColor();
#endif
            // TODO: Support border widths on all sides
            BorderWidth = box.Dimensions.Border.Top;
        }

#if Windows
        public Drawing.Color Color { get; }
        public Drawing.Color BorderColor { get; }
#elif Linux
        public Gdk.Color Color { get; }
        public Gdk.Color BorderColor { get; }
#endif
        public float BorderWidth { get; }
    }
}
