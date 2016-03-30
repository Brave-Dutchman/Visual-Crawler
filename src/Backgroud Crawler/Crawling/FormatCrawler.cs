using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Core;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public class FormatCrawler : Threaded
    {
        private readonly List<CrawledLink> _crawled;
        private readonly List<Link> _links;

        public FormatCrawler()
        {
            _crawled = new List<CrawledLink>();
            _links = new List<Link>();
        }

        public override void Run()
        {
            while (!Stop)
            {
                CrawledContent crawled = CrawlingStorage.GetNewContent();

                if (crawled != null)
                {
                    Format(crawled);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public void Format(CrawledContent crawledContent)
        {
            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");

            MatchCollection matches = regexLink.Matches(crawledContent.Content);

            foreach (object match in matches)
            {
                string url = match.ToString();
                if (url.StartsWith("#") || Path.HasExtension(url) || url.Equals("/")) continue;

                if (url.StartsWith("//"))
                {
                    url = crawledContent.Scheme + ":" + url;
                }

                if (!url.StartsWith("http"))
                {
                    url = crawledContent.Header + url;
                }

                if (crawledContent.Header == url || url.Contains("?") || url.LastIndexOf(":", StringComparison.Ordinal) > 6) continue;

                if (!_crawled.ContainsCrawled(url) && !Storage.CheckCrawledLinksDouble(url))
                {
                    _crawled.Add(new CrawledLink(url));
                }

                if (_links.ContainsLink(crawledContent.Url, url))
                {
                    _links.FindLink(crawledContent.Url, url).TimesOnPage++;
                }
                else
                {
                    _links.Add(new Link(crawledContent.Host, crawledContent.Url, url));
                }
            }

            ToDbStorage.Add(_crawled);
            _crawled.Clear();

            ToDbStorage.Add(_links);
            _links.Clear();

            Console.WriteLine("Formated: {0}\n", crawledContent.Url);
        }
    }
}
