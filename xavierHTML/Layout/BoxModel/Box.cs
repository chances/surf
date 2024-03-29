using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using xavierHTML.CSS.Properties;
using xavierHTML.CSS.Style;

namespace xavierHTML.Layout.BoxModel
{
    public class Box
    {
        public Box(ReadOnlyCollection<Box> children)
        {
            // initially set all fields to 0.0
            Dimensions = default(Dimensions);
            Children = children;
        }

        public Dimensions Dimensions;

        /// <summary>
        /// A generic box is an anonymous block box.
        /// </summary>
        public Display Display => Display.Block;

        public ReadOnlyCollection<Box> Children { get; }

        /// <summary>
        /// Build a new layout tree, deferring layout calculations.
        /// </summary>
        /// <param name="rootNode">A <see cref="xavierHTML.DOM.Document"/>'s root <see cref="StyledNode"/>.</param>
        /// <returns>Fully built layout tree.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when given a <see cref="StyledNode"/> with display property set to none.
        /// </exception>
        public static Box FromStyledNode(StyledNode rootNode)
        {
            if (rootNode.Display == Display.None)
                throw new InvalidOperationException("Root node has display: none.");

            var nodes = rootNode.Children
                // Skip nodes with `display: none;`
                .Where(node => node.Display != Display.None)
                .Select(FromStyledNode).ToList();

            // If a block-level node has only inline children, return just this block-level box with
            // the inline children.
            if (rootNode.Display == Display.Block &&
                 nodes.TrueForAll(box => box.Display == Display.Inline))
                return new NodeBox(nodes.AsReadOnly(), rootNode);
            
            // TODO: Handle case where an inline node contains a block-level child
            // CSS2 box generation algorithm
            // http://www.w3.org/TR/CSS2/visuren.html#box-gen

            // If a block node contains an inline child, create an anonymous block box to contain it.
            // If there are several inline children in a row, put them all in the same anonymous container.
            var children = new List<Box>();
            nodes.ForEach(box =>
            {
                if (box.Display != Display.Inline) children.Add(box);

                var anonymousBox = children.LastOrDefault(b => !(b is NodeBox));
                if (anonymousBox != null)
                    children.Add(new Box(anonymousBox.Children.Append(box).ToList().AsReadOnly()));
                
                children.Add(new Box(new List<Box> { box }.AsReadOnly()));
            });

            return new NodeBox(children.AsReadOnly(), rootNode);
        }

        /// <summary>
        /// Lay out a box and its descendants.
        /// </summary>
        /// <param name="viewport">Size of this box's container.</param>
        public void Layout(Size viewport)
        {
            switch (Display)
            {
                case Display.Inline:
                    // TODO: Implement inline box layout algorithm
                    break;
                case Display.Block:
                    this.LayoutBlock(viewport, new Rectangle(
                        viewport.Width, 0
                    ));
                    break;
                case Display.Flex:
                    // TODO: Implement flex box layout algorithm
                    break;
                default:
                    return;
            }
            
            Console.WriteLine($"Total height: {Dimensions.BorderBox.Height}");
        }
    }

    public class NodeBox : Box
    {
        public NodeBox(ReadOnlyCollection<Box> children, StyledNode style) : base(children)
        {
            Style = style;
        }

        public StyledNode Style { get; }

        public new Display Display => Style.Display;
    }
}
