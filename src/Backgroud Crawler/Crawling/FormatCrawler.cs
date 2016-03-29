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
        List<CrawledLink> crawled = new List<CrawledLink>();
        List<Link> links = new List<Link>();

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

                if (!crawled.ContainsCrawled(url) && !Storage.CheckCrawledLinksDouble(url))
                {
                    crawled.Add(new CrawledLink(url));
                }

                if (links.ContainsLink(crawledContent.Url, url))
                {
                    links.FindLink(crawledContent.Url, url).TimesOnPage++;
                }
                else
                {
                    links.Add(new Link(crawledContent.Host, crawledContent.Url, url));
                }
            }

            ToDbStorage.Add(crawled);
            crawled.Clear();

            ToDbStorage.Add(links);
            links.Clear();

            Console.WriteLine("Formated: {0}\n", crawledContent.Url);
        }
    }
}
