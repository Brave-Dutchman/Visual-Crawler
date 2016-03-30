using System;
using System.IO;
using System.Net;
using System.Threading;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public class WebCrawler : Threaded
    {
        public override void Run()
        {
            while (!Stop)
            {
                if (CrawlingStorage.Count < 10)
                {
                    string url = CrawlingStorage.GetCrawledLink().Link;
                    Crawler(url);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void Crawler(string webUrl)
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
                            CrawlingStorage.AddFound(new CrawledContent(myWebResponse.ResponseUri.ToString(), sreader.ReadToEnd(), myWebResponse));
                            //Console.WriteLine("Crawled: {0}\n", myWebResponse.ResponseUri); //Reads it to the end
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