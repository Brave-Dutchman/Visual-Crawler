namespace Visual_Crawl
{
    public class ParentChild
    {
        public VisualLink Parent { get; set; }
        public VisualLink VisualLink { get; set; }
        public int ChildIndex { get; set; }

        public ParentChild(VisualLink parent, VisualLink visualLink, int childIndex)
        {
            ChildIndex = childIndex;
            VisualLink = visualLink;
            Parent = parent;
        }

        public override string ToString()
        {
            return Parent.Link.From + " - " + VisualLink.Link.To;
        }
    }
}
