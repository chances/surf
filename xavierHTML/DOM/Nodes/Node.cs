using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using xavierHTML.DOM.Elements;

namespace xavierHTML.DOM.Nodes
{
    public abstract class Node
    {
        protected Node(List<Node> children = null)
        {
            Children = children == null
                ? new ObservableCollection<Node>()
                : new ObservableCollection<Node>(children);
            Children.CollectionChanged += ChildrenChanged;
        }

        public Document OwnerDocument { get; protected set; }
        public Node Parent { get; private set; }

        public Element ParentElement => Parent is Element element ? element : null;

        public ObservableCollection<Node> Children { get; }

        public string TextContent => _concatenateTextNodes();

        private string _concatenateTextNodes()
        {
            return Children.Aggregate("", (s, node) =>
            {
                switch (node)
                {
                    case TextNode textNode:
                        s += textNode.Data;
                        break;
                    case Element element:
                        s += element.TextContent;
                        break;
                }

                return s;
            });
        }

        private void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ((Node) e.NewItems[0]).Parent = this;
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ((Node) e.OldItems[0]).Parent = this;
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ((Node) e.NewItems[0]).Parent = this;
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var oldChild in e.OldItems.OfType<Node>())
                    {
                        oldChild.Parent = null;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class EmptyNode : Node
    {
    }
}
