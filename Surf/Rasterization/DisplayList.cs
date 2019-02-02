using System.Collections;
using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS;
using xavierHTML.CSS.Style;
using xavierHTML.DOM;
using xavierHTML.Layout;
using xavierHTML.Layout.BoxModel;

namespace Surf.Rasterization
{
    public class DisplayList : IReadOnlyList<DisplayCommand>
    {
        private readonly List<DisplayCommand> _list = new List<DisplayCommand>();
        private Box _layoutTree;
        private Dimensions _viewport;

        private DisplayList() {}

        public DisplayList(Document document, Dimensions viewport)
        {
            _viewport = viewport;
            
            Render(document);
        }

        public static readonly DisplayList Empty = new DisplayList();

        public IEnumerator<DisplayCommand> GetEnumerator() => _list.AsReadOnly().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => _list.Count;

        public DisplayCommand this[int index] => _list[index];

        public Dimensions Viewport
        {
            get => _viewport;
            set
            {
                _viewport = value;
                _layoutTree?.Layout(_viewport);
            }
        }

        public void Render()
        {
            _list.Clear();
            Render(_layoutTree);
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
            var styleTree = StyledNode.FromElement(document.DocumentElement, styleRules);
            _layoutTree = Box.FromStyledNode(styleTree);
            
            Render();
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
