using xavierHTML.CSS.Values;
#if Windows
using Drawing = System.Drawing;
#elif OSX
using CoreGraphics;
#endif

namespace Surf.Rasterization
{
    public static class ColorExtensions
    {
#if Windows
        public static Drawing.Color ToColor(this Color color)
        {
            return Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
#elif OSX
        public static CGColor ToCGCOlor(this Color color)
        {
            return new CGColor(
                (float) (color.R / 255.0),
                (float) (color.G / 255.0),
                    (float) (color.B / 255.0),
                        (float) (color.A / 255.0)
            );
        }
#endif
    }
}
