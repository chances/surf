namespace xavierHTML.Layout
{
    public struct Dimensions
    {
        public Dimensions(Rectangle content, EdgeSizes margin, EdgeSizes border, EdgeSizes padding)
        {
            Content = content;
            Margin = margin;
            Border = border;
            Padding = padding;
        }

        /// <summary>
        /// Position of a box's content area relative to the document origin.
        /// </summary>
        public Rectangle Content;

        public EdgeSizes Margin;
        public EdgeSizes Border;
        public EdgeSizes Padding;

        public Rectangle PaddingBox => Content.ExpandedBy(Padding);
        public Rectangle BorderBox => PaddingBox.ExpandedBy(Padding);
        public Rectangle MarginBox => BorderBox.ExpandedBy(Margin);
    }
}
