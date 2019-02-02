using System;
using CoreGraphics;
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
            Color = box.Style.BackgroundColor.ToCGCOlor();
            BorderColor = box.Style.BorderColor.ToCGCOlor();
            // TODO: Support border widths on all sides
            BorderWidth = box.Dimensions.Border.Top;
        }

        public CGColor Color { get; }
        public CGColor BorderColor { get; }
        public nfloat BorderWidth { get; }
    }
}
