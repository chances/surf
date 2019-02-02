namespace xavierHTML.Layout
{
    public struct EdgeSizes
    {
        public EdgeSizes(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public static readonly EdgeSizes Zero = new EdgeSizes(0, 0, 0, 0);

        public float Left;
        public float Right;
        public float Top;
        public float Bottom;
    }
}
