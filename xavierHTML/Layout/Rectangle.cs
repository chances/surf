namespace xavierHTML.Layout
{
    public struct Rectangle
    {
        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public Rectangle ExpandedBy(EdgeSizes edges)
        {
            return new Rectangle(
                X - edges.Left,
                Y - edges.Top,
                Width + edges.Left + edges.Right,
                Height + edges.Top + edges.Bottom
            );
        }
    }
}
