using System;
using System.Collections.Generic;
using System.Linq;
using xavierHTML.CSS.Selectors;
using xavierHTML.CSS.Values;
using xavierHTML.DOM;
using xavierHTML.DOM.Elements;

namespace xavierHTML.CSS.Style
{
    public class StyledNode
    {
        public StyledNode(Node node)
        {
            Node = node;
            SpecifiedValues = new Dictionary<string, List<Value>>();
            Children = new List<StyledNode>();
        }

        public Node Node { get; }
        public Dictionary<string, List<Value>> SpecifiedValues { get; }

        public List<StyledNode> Children { get; }

        public static StyledNode FromElement(Element rootElement, List<Rule> rules)
        {
            var node = new StyledNode(rootElement);
            var matchedRules = rules.Select(rule => MatchRule(rootElement, rule)).Where(t => t != null).ToList();
            // Sort matched rules by specificity, low to high
            matchedRules.Sort((matchA, matchB) => Specificity.CompareSelectors(matchA.Item1, matchB.Item1));
            foreach (var matchedRule in matchedRules)
            {
                foreach (var declaration in matchedRule.Item2.Declarations)
                {
                    node.SpecifiedValues[declaration.Name] = declaration.Values;
                }
            }

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
    }
}
