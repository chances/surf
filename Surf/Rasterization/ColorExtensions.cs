using CoreGraphics;
using xavierHTML.CSS.Values;

namespace Surf.Rasterization
{
    public static class ColorExtensions
    {
        public static CGColor ToCGColor(this Color color)
        {
            return new CGColor(
                (float)(color.R / 255.0),
                (float)(color.G / 255.0),
                (float)(color.B / 255.0),
                (float)(color.A / 255.0)
            );
        }
    }
}
