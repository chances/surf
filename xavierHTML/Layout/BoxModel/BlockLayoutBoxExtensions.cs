using System;
using System.Linq;
using xavierHTML.CSS.Values;

namespace xavierHTML.Layout.BoxModel
{
    public static class BlockLayoutBoxExtensions
    {
        public static void LayoutBlock(this Box box, Dimensions container)
        {
            // Child width can depend on parent width, so we need to calculate
            // this box's width before laying out its children.
            box.CalculateWidth(container);

            // Determine where the box is located within its container.
            box.CalculatePosition(container);

            // Recursively lay out the children of this box.
            box.LayoutChildren();

            // Parent height can depend on child height, so `calculate_height`
            // must be called *after* the children are laid out.
            box.CalculateHeight();
        }

        private static void CalculateWidth(this Box box, Dimensions container)
        {
            if (!(box is NodeBox styledBox))
                throw new InvalidOperationException("Anonymous block box has no style node");
            var style = styledBox.Style;

            var width = style.GetValue("width") ?? Keyword.Auto;
            
            // margin, border width, and padding have initial value 0
            var zero = Length.Zero<Pixels>();

            var margins = style.Margins;
            var borderWidths = style.BorderWidths;
            var paddings = style.Paddings;

            // Minimum horizontal space needed for the box
            var total = (new[]
            {
                margins.Left, margins.Right,
                borderWidths.Left, borderWidths.Right,
                paddings.Left, paddings.Right,
                width
            }).Select(v => v.ToPixels()).Sum();
            
            var widthIsAuto = width is Keyword autoWidth && autoWidth == Keyword.Auto;
            var marginLeftIsAuto = margins.Left is Keyword marginLeft && marginLeft == Keyword.Auto;
            var marginRightIsAuto = margins.Right is Keyword marginRight && marginRight == Keyword.Auto;
            
            // If width is not auto and the total is wider than the container, treat auto margins as 0
            if (!widthIsAuto && total > container.Content.Width)
            {
                if (marginLeftIsAuto)
                    margins.Left = zero;
                if (marginRightIsAuto)
                    margins.Right = zero;
            }
            
            var underflow = container.Content.Width - total;
            var underflowLength = new Length(underflow, Unit.Pixels);
            
            // If the values are overconstrained, calculate margin_right
            if (!widthIsAuto && !marginLeftIsAuto && !marginRightIsAuto)
                margins.Right = new Length(margins.Right.ToPixels() + underflow, Unit.Pixels);
            
            // If exactly one size is auto, its used value follows from the equality
            if (!widthIsAuto && !marginLeftIsAuto && marginRightIsAuto)
                margins.Right = underflowLength;
            if (!widthIsAuto && marginLeftIsAuto && !marginRightIsAuto)
                margins.Left = underflowLength;
            
            // If width is set to auto, any other auto values become 0
            if (widthIsAuto)
            {
                if (marginLeftIsAuto)
                    margins.Left = zero;
                if (marginRightIsAuto)
                    margins.Right = zero;

                if (underflow > 0.0)
                {
                    // Expand width to fill the underflow
                    width = underflowLength;
                }
                else
                {
                    // Width can't be negative. Adjust the right margin instead
                    width = zero;
                    margins.Right = new Length(margins.Right.ToPixels() + underflow, Unit.Pixels);
                }
            }
            
            // If margin-left and margin-right are both auto, their used values are equal
            if (!widthIsAuto && marginLeftIsAuto && marginRightIsAuto)
            {
                var underflowHalfLength = new Length(underflow / 2.0, Unit.Pixels);
                margins.Left = margins.Right = underflowHalfLength;
            }

            // Constraints are met and any auto values have been converted to lengths
            box.Dimensions.Content.Width = width.ToPixels();
            
            box.Dimensions.Margin.Left = margins.Left.ToPixels();
            box.Dimensions.Margin.Right = margins.Right.ToPixels();
            
            box.Dimensions.Border.Left = borderWidths.Left.ToPixels();
            box.Dimensions.Border.Right = borderWidths.Right.ToPixels();
            
            box.Dimensions.Padding.Left = paddings.Left.ToPixels();
            box.Dimensions.Padding.Right = paddings.Right.ToPixels();
        }

        private static void CalculatePosition(this Box box, Dimensions container)
        {
            if (!(box is NodeBox styledBox))
                throw new InvalidOperationException("Anonymous block box has no style node");
            var style = styledBox.Style;

            var margins = style.Margins;
            var borderWidths = style.BorderWidths;
            var paddings = style.Paddings;

            box.Dimensions.Margin.Top = margins.Top.ToPixels();
            box.Dimensions.Margin.Bottom = margins.Bottom.ToPixels();

            box.Dimensions.Border.Top = borderWidths.Top.ToPixels();
            box.Dimensions.Border.Bottom = borderWidths.Bottom.ToPixels();

            box.Dimensions.Padding.Top = paddings.Top.ToPixels();
            box.Dimensions.Padding.Bottom = paddings.Bottom.ToPixels();

            box.Dimensions.Content.X = container.Content.X +
                                       box.Dimensions.Margin.Left +
                                       box.Dimensions.Border.Left +
                                       box.Dimensions.Padding.Left;

            // Position the box below all the previous boxes in the container
            box.Dimensions.Content.Y = container.Content.Height +
                                       container.Content.Y +
                                       box.Dimensions.Margin.Top +
                                       box.Dimensions.Border.Top +
                                       box.Dimensions.Padding.Top;
        }

        private static void LayoutChildren(this Box box)
        {
            foreach (var child in box.Children)
            {
                child.LayoutBlock(box.Dimensions);
                // Track the height so each child is laid out below the previous content
                box.Dimensions.Content.Height += child.Dimensions.MarginBox.Height;
            }
        }

        private static void CalculateHeight(this Box box)
        {
            if (!(box is NodeBox styledBox))
                throw new InvalidOperationException("Anonymous block box has no style node");
            var style = styledBox.Style;
            
            // If the height is set to an explicit length, use that exact length
            // Otherwise, just keep the value set by LayoutChildren
            var height = style.GetValue("height") ?? Keyword.Auto;
            if (!(height is Keyword autoHeight && autoHeight == Keyword.Auto) &&
                height is Length length && length.Unit == Unit.Pixels)
            {
                box.Dimensions.Content.Height = length.ToPixels();
            }
        }
    }
}
