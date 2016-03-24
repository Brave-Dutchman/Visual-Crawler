namespace Visual_Crawl
{
    public class ParentChild
    {
        public VisualLink Parent { get; set; }

        public VisualLink VisualLink { get; set; }

        public ParentChild(VisualLink parent, VisualLink visualLink)
        {
            VisualLink = visualLink;
            Parent = parent;
        }
    }
}
