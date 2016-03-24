using System.Collections.Generic;

namespace Visual_Crawl
{
    public class ParentChildren
    {
        public VisualLink Parent { get; set; }

        public List<VisualLink> VisualLinks { get; set; }

        public ParentChildren(VisualLink parent, VisualLink child)
        {
            Parent = parent;
            VisualLinks = new List<VisualLink> {child};
        }
    }
}
