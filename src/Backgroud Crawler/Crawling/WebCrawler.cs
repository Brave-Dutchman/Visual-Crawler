﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Backgroud_Crawler._Storage;
using Core.Data;

namespace Backgroud_Crawler.Crawling
{
    /// <summary>
    /// The WebCrawler class
    /// Responsible for the actual crawling process
    /// </summary>
    public class WebCrawler
    {
        //Fields
        public bool Stop { get; set; } 

        /// <summary>
        /// Run the WebCrawler
        /// </summary>
        public void Run()
        {
            while (!Stop)
            {
                CrawledLink craweledLink = FromStorage.GetCrawledLink();

                if (craweledLink == null)
                {
                    Console.WriteLine("The web is crawled");
                    break;
                }

                Crawler(craweledLink.Link);
            }
        }

        /// <summary>
        /// The actual Crawler
        /// </summary>
        /// <param name="webUrl">String to crawl</param>
        public void Crawler(string webUrl)
        {
            string text;
            string host;
            string scheme;
            string url;

            try
            {
                WebRequest myWebRequest = WebRequest.Create(webUrl);
                using (WebResponse myWebResponse = myWebRequest.GetResponse())
                {
                    using (Stream streamResponse = myWebResponse.GetResponseStream())
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        using (StreamReader sreader = new StreamReader(streamResponse))
                        {
                            url = myWebResponse.ResponseUri.ToString();
                            if (url.EndsWith("/"))
                            {
                                url = url.Substring(0, url.Length - 1);
                            }

                            Console.WriteLine("Crawled:   {0}", url); //Reads it to the end

                            text = sreader.ReadToEnd();
                        }
                    }

                    host = myWebResponse.ResponseUri.Host;
                    scheme = myWebResponse.ResponseUri.Scheme;
                }

                Task.Run(() => 
                {
                    new ProcessCrawled(new CrawledContent(url, text, host, scheme)).Process();
                });
            }
            catch (Exception)
            {
                //Console.WriteLine("\t{0}:{1}", webUrl, e.Message);
            }
        }
    }
}