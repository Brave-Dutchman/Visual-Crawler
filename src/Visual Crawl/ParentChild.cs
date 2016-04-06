using Core.Data;

namespace Visual_Crawl
{
    public class ParentChild
    {
        public Link Parent { get; set; }
        public Link VisualLink { get; set; }
        public int ChildIndex { get; set; }

        public ParentChild(Link parent, Link visualLink, int childIndex)
        {
            ChildIndex = childIndex;
            VisualLink = visualLink;
            Parent = parent;
        }

        public override string ToString()
        {
            return Parent.From + " - " + VisualLink.To;
        }
    }
}
