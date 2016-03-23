using Core;

namespace Visual_Crawl
{
    public class LinkNode
    {
        public Link Link { get; set; }
        public LinkNode[] Nodes { get; set; }

        public LinkNode(Link link)
        {
            Link = link;
        }
    }
}
