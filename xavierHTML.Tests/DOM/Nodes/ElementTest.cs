using xavierHTML.DOM;
using xavierHTML.DOM.Elements;
using Xunit;

namespace xavierHTML.Tests.DOM.Nodes
{
    public class ElementTest
    {
        [Fact]
        public void NodeHasParentTest()
        {
            var parent = new Element("node");
            var child = new Element("node");
            parent.Children.Add(child);

            Assert.True(child.Parent.Equals(parent));
            Assert.True(child.ParentElement.Equals(parent));
        }
    }
}
