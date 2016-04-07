using System.Collections.Generic;
using System.Linq;

namespace Core.Data
{
    /// <summary>
    /// Extension class
    /// </summary>
    public static class Extends
    {
        /// <summary>
        /// Check if link exists in a given collection
        /// </summary>
        /// <param name="crawledList">Collection of Crawled Links</param>
        /// <param name="contains">Link (URL) to check for</param>
        /// <returns>Return true if link exists within given collection</returns>
        public static bool ContainsCrawled(this List<CrawledLink> crawledList, string contains)
        {
            return crawledList.Any(crawled => crawled.Link == contains);
        }

        /// <summary>
        /// Check if link exists in a given collection
        /// </summary>
        /// <param name="links">Link (URL)</param>
        /// <param name="from">Source URL</param>
        /// <param name="to">Destination URL</param>
        /// <returns>Exists true/false</returns>
        public static bool ContainsLink(this List<Link> links, string from, string to)
        {
            return links.Any(link => link.From == from && link.To == to);
        }

        /// <summary>
        /// Find a Link within the given collection
        /// </summary>
        /// <param name="links">Link (URL)</param>
        /// <param name="from">Source URL</param>
        /// <param name="to">Destination URL</param>
        /// <returns>Is found true/false</returns>
        public static Link FindLink(this List<Link> links, string from, string to)
        {
            return links.FirstOrDefault(link => link.From == from && link.To == to);
        }
    }
}
