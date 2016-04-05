using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public class FormatCrawler
    {
        private readonly List<CrawledLink> _crawled;
        private readonly List<Link> _links;

        private CrawledContent _crawledContent;

        public FormatCrawler()
        {
            _crawled = new List<CrawledLink>();
            _links = new List<Link>();
        }

        public void Set(CrawledContent crawledContent)
        {
            _crawledContent = crawledContent;
        }

        public void Format(object b)
        {
            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");
            MatchCollection matches = regexLink.Matches(_crawledContent.Content);

            foreach (object match in matches)
            {
                string url = match.ToString();

                if(url.Equals("/")) continue;
                else if (_crawledContent.Header == url) continue;
                else if (url.Contains("?")) continue;
                else if (url.Contains("#")) continue;
                else if (url.Contains(";")) continue;
                else if (url.LastIndexOf(":", StringComparison.Ordinal) > 6) continue;

                if (url.StartsWith("//"))
                {
                    url = _crawledContent.Scheme + ":" + url;
                }
                else if (!url.StartsWith("http"))
                {
                    url = _crawledContent.Header + url;
                }

                if (url.EndsWith("/"))
                {
                    url = url.Substring(0, url.Length - 1);
                }

                if (!_crawled.ContainsCrawled(url))
                {
                    _crawled.Add(new CrawledLink(url));
                }

                if (_links.ContainsLink(_crawledContent.Url, url))
                {
                    _links.FindLink(_crawledContent.Url, url).TimesOnPage++;
                }
                else
                {
                    _links.Add(new Link(_crawledContent.Host, _crawledContent.Url, url));
                }
            }

            ToStorage.Add(_crawled, _crawledContent.Url);
            ToStorage.Add(_links);

            Console.WriteLine("Formated: {0}", _crawledContent.Url);
        }
    }
}
