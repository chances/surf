using System;
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
            Color = box.Style.BackgroundColor.ToGdkColor();
            BorderColor = box.Style.BorderColor.ToGdkColor();
            // TODO: Support border widths on all sides
            BorderWidth = box.Dimensions.Border.Top;
        }

        public Gdk.Color Color { get; }
        public Gdk.Color BorderColor { get; }
        public float BorderWidth { get; }
    }
}
