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
        public Rectangle Content { get; }
        
        public EdgeSizes Margin { get; }
        public EdgeSizes Border { get; }
        public EdgeSizes Padding { get; }
    }
}
