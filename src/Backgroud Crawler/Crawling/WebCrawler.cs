using System;
using System.IO;
using System.Net;
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

                if (craweledLink != null)
                {
                    Crawler(craweledLink.Link);
                }
                else
                {
                    Console.WriteLine("\n{0}", "Saving the old links");
                    ToDbStorage.Write();
                    Console.WriteLine("{0}\n", "Getting new links");
                    CrawlingStorage.GetNewLinks();
                }
            }
        }

        private void Crawler(string webUrl)
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

                            Console.WriteLine("Crawled: {0}", url); //Reads it to the end

                            FormatCrawler crawler = new FormatCrawler();
                            crawler.Set(new CrawledContent(url, sreader.ReadToEnd(), myWebResponse));
                            crawler.Format();
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