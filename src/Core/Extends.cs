using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public static class Extends
    {
        public static bool ContainsCrawled(this List<CrawledLink> crawled, string contains)
        {
            return crawled.Any(link => link.Link == contains);
        }

        public static bool ContainsLink(this List<Link> links, string from, string to)
        {
            return links.Any(link => link.From == from && link.To == to);
        }

        public static Link FindLink(this List<Link> links, string from, string to)
        {
            return links.FirstOrDefault(link => link.From == from && link.To == to);
        }
    }
}
