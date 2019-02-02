using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS;
using xavierHTML.CSS.Style;
using xavierHTML.DOM;
using xavierHTML.DOM.Elements;
using xavierHTML.Layout;
using xavierHTML.Layout.BoxModel;

namespace Surf.Rasterization
{
    public class DisplayList : IReadOnlyList<DisplayCommand>
    {
        private readonly List<DisplayCommand> _list = new List<DisplayCommand>();
        private StyledNode _styleTree;
        private Rectangle _viewport;

        private DisplayList() {}

        public DisplayList(Document document, Rectangle viewport)
        {
            _viewport = viewport;
            
            Render(document);
        }

        public static readonly DisplayList Empty = new DisplayList();

        public IEnumerator<DisplayCommand> GetEnumerator() => _list.AsReadOnly().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _list.Count;

        public DisplayCommand this[int index] => _list[index];

        public Rectangle Viewport
        {
            get => _viewport;
            set
            {
                _viewport = value;
                LayoutAndRender();
            }
        }

        public void Render(Document document)
        {
            var styleRules = document.Stylesheets.Aggregate(
                new List<Rule>(),
                (rules, stylesheet) =>
                {
                    rules.AddRange(stylesheet.Rules);
                    return rules;
                });
            _styleTree = StyledNode.FromElement(document.DocumentElement, styleRules);
            
            LayoutAndRender();
        }

        private void LayoutAndRender()
        {
            _list.Clear();
            if (_styleTree == null) return;

            var layoutTree = Box.FromStyledNode(_styleTree);
            PrintLayoutTree(layoutTree);
            layoutTree.Layout(_viewport.Size);
            Render(layoutTree);
        }

        private void PrintLayoutTree(Box box, int level = 0)
        {
            var label = box is NodeBox node
                    ? node.Style.Node is Element element
                        ? element.TagName
                        : node.Style.Node.ToString()
                    : "[Anon]";
            Console.WriteLine(label.PadLeft(level));
            foreach (var child in box.Children)
            {
                PrintLayoutTree(child, level += 1);
            }
        }

        private void Render(Box box)
        {
            if (box == null) return;

            var isBoxOpaque = box is NodeBox nodeBox &&
                              nodeBox.Style.BackgroundColor.A > 0 &&
                              nodeBox.Style.BorderColor.A > 0;

            if (isBoxOpaque)
                _list.Add(new SolidColor((NodeBox) box));

            foreach (var child in box.Children)
            {
                Render(child);
            }
        }
    }
}
