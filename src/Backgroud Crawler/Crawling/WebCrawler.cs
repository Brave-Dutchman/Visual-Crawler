using System;
using System.IO;
using System.Net;
using System.Threading;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public class WebCrawler : Threaded
    {
        public void Run()
        {
            while (!Stop)
            {
                CrawledLink craweledLink = CrawlingStorage.GetCrawledLink();

                if (craweledLink == null)
                {
                    Console.WriteLine("The web is crawled");
                    break;
                }

                Crawler(craweledLink.Link);
            }
        }

        public void Crawler(string webUrl)
        {
            try
            {
                WebRequest myWebRequest = WebRequest.Create(webUrl);
                using (WebResponse myWebResponse = myWebRequest.GetResponse())
                {
                    using (Stream streamResponse = myWebResponse.GetResponseStream())
                    {
                        using (StreamReader sreader = new StreamReader(streamResponse))
                        {
                            string url = myWebResponse.ResponseUri.ToString();
                            if (url.EndsWith("/"))
                            {
                                url = url.Substring(0, url.Length - 1);
                            }

                            //Console.WriteLine("Crawled:  {0}", url); //Reads it to the end

                            FormatCrawler crawler = new FormatCrawler();
                            crawler.Set(new CrawledContent(url, sreader.ReadToEnd(), myWebResponse));

                            ThreadPool.QueueUserWorkItem(crawler.Format);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t{0}:{1}", webUrl, e.Message);
            }
        }
    }
}