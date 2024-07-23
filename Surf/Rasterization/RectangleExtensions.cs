#if OSX
using CoreGraphics;
#endif
using xavierHTML.Layout;

namespace Surf.Rasterization
{
    public static class RectangleExtensions
    {
#if OSX
        public static CGRect ToCgRect(this Rectangle rectangle)
        {
            return new CGRect(
                rectangle.X,
                rectangle.Y,
                rectangle.Width,
                rectangle.Height
            );
        }
#endif
    }
}
