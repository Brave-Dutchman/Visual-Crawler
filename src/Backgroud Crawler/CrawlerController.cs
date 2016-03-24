using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Backgroud_Crawler
{
    public class CrawlerController
    {
        private readonly PerformanceCounter _cpuCounter;

        private static List<WebCrawler> WebCrawlers { get; set; }

        private bool _stop;

        public CrawlerController()
        {
            WebCrawlers = new List<WebCrawler> { new WebCrawler(1), new WebCrawler(2), new WebCrawler(3), new WebCrawler(4) };
 
            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
        }

        public float GetCurrentCpuUsage()
        {
            return _cpuCounter.NextValue();
        }

        public void Start()
        {
            foreach (WebCrawler item in WebCrawlers)
            {
                StartTask(item);
            }

            while (!_stop)
            {
                float cpu = GetCurrentCpuUsage();

                if (cpu > 80)
                {
                    if (WebCrawlers.Count > 0)
                    {
                        WebCrawlers[WebCrawlers.Count -1].Stop = true;
                        WebCrawlers.RemoveAt(WebCrawlers.Count - 1);
                        Console.WriteLine("Removed a crawler, {0}", WebCrawlers.Count);
                    }
                }
                else if(cpu < 50)
                {
                    if (WebCrawlers.Count < 20)
                    {
                        WebCrawler crawler = new WebCrawler(WebCrawlers.Count + 1);
                        WebCrawlers.Add(crawler);
                        StartTask(crawler);
                        Console.WriteLine("Created new Crawler, {0}", WebCrawlers.Count);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void StartTask(WebCrawler crawler)
        {
            Task.Run(() => { crawler.Run(); });
        }

        public void Stop()
        {
            _stop = true;

            foreach (WebCrawler webCrawler in WebCrawlers)
            {
                webCrawler.Stop = true;
            }
        }
    }
}
