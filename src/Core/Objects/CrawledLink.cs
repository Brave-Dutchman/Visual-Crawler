﻿using Newtonsoft.Json;

namespace Core.Objects
{
    public class CrawledLink
    {
        public string Link { get; set; }

        public bool IsCrawled { get; set; }

        [JsonConstructor]
        public CrawledLink() { }

        public CrawledLink(string link)
        {
            Link = link;
            IsCrawled = false;
        }

        public CrawledLink(string link, bool isCrawled)
        {
            Link = link;
            IsCrawled = isCrawled;
        }

        public override string ToString()
        {
            return Link;
        }
    }
}
