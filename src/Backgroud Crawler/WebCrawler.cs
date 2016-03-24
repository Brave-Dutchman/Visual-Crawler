﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Core;

namespace Backgroud_Crawler
{
    public class WebCrawler
    {
        private const string START_URL = "http://github.com/";
        
        private readonly int _number;

        public bool Stop { get; set; }

        public WebCrawler(int number)
        {
            _number = number;
            Stop = false;
        }

        public void FirstCrawl()
        {
            Crawler(START_URL);
        }

        public void Run()
        {
            while (!Stop)
            {
                string url = CrawledList.GetCrawledLink().Link;
                Crawler(url);
            }
        }

        private void Crawler(string webUrl)
        {
            try
            {
                List<CrawledLink> crawled = new List<CrawledLink>();
                List<Link> links = new List<Link>();

                WebRequest myWebRequest = WebRequest.Create(webUrl);

                using (WebResponse myWebResponse = myWebRequest.GetResponse())
                {
                    using (Stream streamResponse = myWebResponse.GetResponseStream())
                    {
                        string header = myWebResponse.ResponseUri.Scheme + "://" + myWebResponse.ResponseUri.Host;

                        Console.WriteLine("{0}:{1}\n", _number, myWebResponse.ResponseUri);

                        using (StreamReader sreader = new StreamReader(streamResponse))
                        {
                            string content = sreader.ReadToEnd(); //reads it to the end
                            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");

                            foreach (object match in regexLink.Matches(content))
                            {
                                string url = match.ToString();
                                if (url.StartsWith("#") || Path.HasExtension(url) || url.Equals("/")) continue;

                                if (!url.StartsWith("http"))
                                {
                                    url = header + url;
                                }

                                crawled.Add(new CrawledLink(url));
                                links.Add(new Link(myWebResponse.ResponseUri.Host, webUrl, url));
                            }
                        }
                    }
                }

                Storage.WriteLinks(crawled);
                Storage.WriteLinks(links);
            }
            catch (Exception e)
            {
                Console.WriteLine("\t{0}:{1}", webUrl, e.Message);
            }
        }
    }
}
