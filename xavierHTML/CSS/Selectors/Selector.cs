using System.Linq;
using Sprache;
using xavierHTML.Parsers.CSS;

namespace xavierHTML.CSS.Selectors
{
    public abstract class Selector
    {
        protected abstract Selector Combine(Selector other);
    }

    public class SimpleSelector : Selector
    {
        public SimpleSelector() : this(null, null, new string[] { })
        {
        }

        public SimpleSelector(string[] classes) : this(null, null, classes)
        {
        }

        public SimpleSelector(string tagName, string id, string[] classes)
        {
            TagName = tagName;
            Id = id;
            Classes = classes;
        }

        public static SimpleSelector FromTagName(string tagName)
        {
            return new SimpleSelector(tagName, null, new string[] { });
        }

        public static SimpleSelector FromId(string id)
        {
            return new SimpleSelector(null, id, new string[] { });
        }

        public string TagName { get; }
        public bool IsUniversalSelector => TagName == "*";
        public string Id { get; }
        public string[] Classes { get; }
        
        private static Parser<SimpleSelector> _tag =
            from t in Tokens.Identifier.Or(Sprache.Parse.String("*").Text())
            select SimpleSelector.FromTagName(t);

        private static readonly Parser<SimpleSelector> _id = Tokens.PrefixedIdentifier('#')
            .Select(SimpleSelector.FromId);
        private static readonly Parser<SimpleSelector> _class =
            Tokens.PrefixedIdentifier('.').Select(s => new SimpleSelector(new[] {s}));

        private static readonly Parser<SimpleSelector> _idOrClass = _id.Or(_class);

        public static readonly Parser<SimpleSelector> Parse =
        (
            from tag in _tag
            from rest in _idOrClass.Many()
            select rest.Append(tag)
        ).Or(_idOrClass.AtLeastOnce()).Select(selectors => selectors.Aggregate(CombineSelectors));

        private static SimpleSelector CombineSelectors(SimpleSelector s1, SimpleSelector s2)
        {
            return new SimpleSelector(s2.TagName ?? s1.TagName, s2.Id ?? s1.Id,
                s1.Classes.Concat(s2.Classes).ToArray());
        }

        protected override Selector Combine(Selector other)
        {
            if (other is SimpleSelector selector)
            {
                return CombineSelectors(this, selector);
            }

            return other;
        }
    }
}
