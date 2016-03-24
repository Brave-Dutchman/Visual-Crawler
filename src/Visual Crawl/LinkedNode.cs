using System.Collections.Generic;

namespace Visual_Crawl
{
    public class LinkedNode
    {
        public VisualLink Data { get; set; }
        public List<LinkedNode> LinkedNodes { get; set; }

        public LinkedNode(VisualLink data)
        {
            Data = data;
            LinkedNodes = new List<LinkedNode>();
        }
    }
}
