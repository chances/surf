using System;
using System.Linq;
using xavierHTML.CSS.Style;
using xavierHTML.CSS.Values;

namespace xavierHTML.Layout.BoxModel
{
    public static class BlockLayoutBoxExtensions
    {
        public static void LayoutBlock(this Box box, Size viewport, Rectangle container)
        {
            // Child width can depend on parent width, so we need to calculate
            // this box's width before laying out its children.
            box.CalculateWidth(viewport.Width);

            // Determine where the box is located within its container.
            box.CalculatePosition(viewport.Height, container);

            // Recursively lay out the children of this box.
            box.LayoutChildren();

            // Parent height can depend on child height, so call CalculateHeight
            // *after* the children are laid out.
            box.CalculateHeight(viewport.Height);
        }

        private static void CalculateWidth(this Box box, float containerWidth)
        {
            var style = box is NodeBox styledBox
                ? styledBox.Style : null;

            var width = style?.GetValue("width") ?? Keyword.Auto;
            
            // margin, border width, and padding have initial value 0
            var zero = Length.Zero<Pixels>();

            // Get margin, border-width, and padding styles from NodeBox
            //  Otherwise, Anonymous block box edges are zero pixels
            var margins = style?.Margins ?? EdgeValues.Zero;
            var borderWidths = style?.BorderWidths ?? EdgeValues.Zero;
            var paddings = style?.Paddings ?? EdgeValues.Zero;

            // Minimum horizontal space needed for the box
            var total = (new[]
            {
                margins.Left, margins.Right,
                borderWidths.Left, borderWidths.Right,
                paddings.Left, paddings.Right,
                width
            }).Select(v => v.ToPixels(containerWidth)).Sum();

            var widthIsAuto = width is Keyword autoWidth && autoWidth == Keyword.Auto;
            var marginLeftIsAuto = margins.Left is Keyword marginLeft && marginLeft == Keyword.Auto;
            var marginRightIsAuto = margins.Right is Keyword marginRight && marginRight == Keyword.Auto;

            // If width is not auto and the total is wider than the container, treat auto margins as 0
            if (!widthIsAuto && total > containerWidth)
            {
                if (marginLeftIsAuto)
                    margins.Left = zero;
                if (marginRightIsAuto)
                    margins.Right = zero;
            }

            var underflow = containerWidth - total;
            var underflowLength = new Length(underflow, Unit.Pixels);

            // If the values are overconstrained, calculate margin_right
            if (!widthIsAuto && !marginLeftIsAuto && !marginRightIsAuto)
                margins.Right = new Length(margins.Right.ToPixels(containerWidth) + underflow, Unit.Pixels);

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
                    margins.Right = new Length(margins.Right.ToPixels(containerWidth) + underflow, Unit.Pixels);
                }
            }

            // If margin-left and margin-right are both auto, their used values are equal
            if (!widthIsAuto && marginLeftIsAuto && marginRightIsAuto)
            {
                var underflowHalfLength = new Length(underflow / 2.0, Unit.Pixels);
                margins.Left = margins.Right = underflowHalfLength;
            }

            // Constraints are met and any auto values have been converted to lengths
            box.Dimensions.Content.Width = width.ToPixels(containerWidth);

            box.Dimensions.Margin.Left = margins.Left.ToPixels(containerWidth);
            box.Dimensions.Margin.Right = margins.Right.ToPixels(containerWidth);

            box.Dimensions.Border.Left = borderWidths.Left.ToPixels(containerWidth);
            box.Dimensions.Border.Right = borderWidths.Right.ToPixels(containerWidth);

            box.Dimensions.Padding.Left = paddings.Left.ToPixels(containerWidth);
            box.Dimensions.Padding.Right = paddings.Right.ToPixels(containerWidth);
        }

        private static void CalculatePosition(this Box box, float containerHeight, Rectangle containerContent)
        {
            var style = box is NodeBox styledBox
                ? styledBox.Style : null;

            // margin, border width, and padding have initial value 0
            //
            // Get margin, border-width, and padding styles from NodeBox
            //  Otherwise, Anonymous block box edges are zero pixels
            var margins = style?.Margins ?? EdgeValues.Zero;
            var borderWidths = style?.BorderWidths ?? EdgeValues.Zero;
            var paddings = style?.Paddings ?? EdgeValues.Zero;

            box.Dimensions.Margin.Top = margins.Top.ToPixels(containerHeight);
            box.Dimensions.Margin.Bottom = margins.Bottom.ToPixels(containerHeight);

            box.Dimensions.Border.Top = borderWidths.Top.ToPixels(containerHeight);
            box.Dimensions.Border.Bottom = borderWidths.Bottom.ToPixels(containerHeight);

            box.Dimensions.Padding.Top = paddings.Top.ToPixels(containerHeight);
            box.Dimensions.Padding.Bottom = paddings.Bottom.ToPixels(containerHeight);

            box.Dimensions.Content.X = containerContent.X +
                                       box.Dimensions.Margin.Left +
                                       box.Dimensions.Border.Left +
                                       box.Dimensions.Padding.Left;

            // Position the box below all the previous boxes in the container
            box.Dimensions.Content.Y = containerContent.Height +
                                       containerContent.Y +
                                       box.Dimensions.Margin.Top +
                                       box.Dimensions.Border.Top +
                                       box.Dimensions.Padding.Top;
        }

        private static void LayoutChildren(this Box box)
        {
            foreach (var child in box.Children)
            {
                child.LayoutBlock(box.Dimensions.Content.Size, box.Dimensions.Content);
                // Track the height so each child is laid out below the previous content
                box.Dimensions.Content.Height += child.Dimensions.PaddingBox.Height;
            }
        }

        private static void CalculateHeight(this Box box, float containerHeight)
        {
            var style = box is NodeBox styledBox ? styledBox.Style : null;
            
            // If the height is set to an explicit length, use that exact length
            // Otherwise, just keep the value set by LayoutChildren
            var height = style?.GetValue("height") ?? Keyword.Auto;

            if (!(height is Keyword autoHeight && autoHeight == Keyword.Auto) &&
                height is Length length && length.Unit != Unit.Unitless)
            {
                box.Dimensions.Content.Height = length.ToPixels(containerHeight);
            }
        }
    }
}
