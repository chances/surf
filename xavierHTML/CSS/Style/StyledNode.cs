using System;
using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS.Properties;
using xavierHTML.CSS.Selectors;
using xavierHTML.CSS.Values;
using xavierHTML.DOM;
using xavierHTML.DOM.Elements;

namespace xavierHTML.CSS.Style
{
    public class StyledNode
    {
        private readonly Lazy<EdgeValues> _margins;
        private readonly Lazy<EdgeValues> _borderWidths;
        private readonly Lazy<EdgeValues> _paddings;
        
        public StyledNode(Node node)
        {
            Node = node;
            SpecifiedValues = new Dictionary<string, List<Value>>();
            Children = new List<StyledNode>();
            
            _margins = new Lazy<EdgeValues>(() => Margin.GetMargins(SpecifiedValues));
            _borderWidths = new Lazy<EdgeValues>(() => BorderWidth.GetBorderWidths(SpecifiedValues));
            _paddings = new Lazy<EdgeValues>(() => Padding.GetPaddings(SpecifiedValues));
        }

        public Node Node { get; }
        public Dictionary<string, List<Value>> SpecifiedValues { get; }

        public List<StyledNode> Children { get; }

        /// <summary>
        /// Value of this Node's `display` property, defaults to Display.Inline.
        /// </summary>
        public Display Display
        {
            get
            {
                var displayProperty = GetValue("display");
                if (displayProperty == null || !(displayProperty is Keyword keyword))
                    return Display.Inline;

                switch (keyword.Value)
                {
                    case "none": return Display.None;
                    case "block": return Display.Block;
                    case "flex": return Display.Flex;
                    default: return Display.Inline;
                }
            }
        }

        public EdgeValues Margins => _margins.Value;

        public EdgeValues BorderWidths => _borderWidths.Value;

        public EdgeValues Paddings => _paddings.Value;

        public Value GetValue(string name) => SpecifiedValues[name]?.FirstOrDefault();

        public static StyledNode FromElement(Element rootElement, List<Rule> rules)
        {
            var node = new StyledNode(rootElement);
            var matchedRules = rules.Select(rule => MatchRule(rootElement, rule)).Where(t => t != null).ToList();
            // Sort matched rules by specificity, low to high
            matchedRules.Sort((matchA, matchB) => Specificity.CompareSelectors(matchA.Item1, matchB.Item1));
            foreach (var matchedRule in matchedRules)
            {
                var rule = matchedRule.Item2;

                // Specify the highest specific rule declarations
                SpecifyDeclarations(node, rule);
            }

            // Specify declarations from the element's style attribute, if any
            rootElement.Style?.Rules
                .ForEach(rule => SpecifyDeclarations(node, rule));

            foreach (var child in rootElement.Children.OfType<Element>())
            {
                node.Children.Add(FromElement(child, rules));
            }

            return node;
        }

        private static Tuple<Selector, Rule> MatchRule(Element element, Rule rule)
        {
            var matchedSelectors = rule.Selectors.Where(element.Matches).ToList();
            if (matchedSelectors.Count == 0) return null;
            matchedSelectors.Sort(Specificity.CompareSelectors);
            return new Tuple<Selector, Rule>(matchedSelectors.LastOrDefault(), rule);
        }

        private static void SpecifyDeclarations(StyledNode node, Rule rule)
        {
            foreach (var declaration in rule.Declarations)
            {
                node.SpecifiedValues[declaration.Name] = declaration.Values;
            }
        }
    }
}
