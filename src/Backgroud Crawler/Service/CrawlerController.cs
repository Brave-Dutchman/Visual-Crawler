﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Backgroud_Crawler.Crawling;
using Backgroud_Crawler._Storage;
using Core.Data;
using Core.Database;

namespace Backgroud_Crawler.Service
{
    /// <summary>
    /// CrawledController class
    /// Manages the WebCrawler service
    /// </summary>
    public class CrawlerController
    {
        //Fields
        private static Timer _timer;
        private readonly PerformanceCounter _cpuCounter;
        private WebCrawler _crawler;
        private bool _isWaiting;

        private const int CPU_HIGH = 60;
        private const int CPU_LOW = 20;

        /// <summary>
        /// Constructor of CrawledController
        /// </summary>
        public CrawlerController()
        {
            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
        }

        /// <summary>
        /// The callback for the timer thread
        /// </summary>
        /// <param name="state"></param>
        private void TimerCallback(object state)
        {
            float cpu = GetCurrentCpuUsage();

            if (!_isWaiting && cpu > CPU_HIGH)
            {
                _isWaiting = true;

                Console.WriteLine("\nStopped Crawling\n");

                _crawler.Stop = true;
            }
            else if (_isWaiting && cpu < CPU_LOW)
            {
                _isWaiting = false;

                Console.WriteLine("\nStarted Crawling again\n");

                _crawler.Stop = false;
                StartCrawling(_crawler);
            }

            Console.WriteLine("\tChecked Cpu, {0}", DateTime.Now.ToLongTimeString());
        }

        /// <summary>
        /// Starts the application by setting the database.
        /// If no database exists creates the first url and runs a crawler on that.
        /// Start the webrawler on a new thread
        /// </summary>
        private void StartUp()
        {
            bool exists = Storage.Enable();

            if (!exists)
            {
                const string url = "http://www.nu.nl";
                Uri uri = new Uri(url);
                
                ToStorage.Add(new List<CrawledLink> { new CrawledLink(url) }, url);
                ToStorage.Add(new List<Link> { new Link(uri.Host, url, url) });

                new WebCrawler().Crawler(url);
                Thread.Sleep(1000);
            }

            _crawler = new WebCrawler();
            StartCrawling(_crawler);
        }

        /// <summary>
        /// Gets the current CPU usage.
        /// As per Microsofts advice first call the counter then wait 1000 ms and the call it again
        /// This way the value will be correct
        /// </summary>
        /// <returns>The current cpu usage in %</returns>
        public float GetCurrentCpuUsage()
        {
            dynamic firstValue = _cpuCounter.NextValue();

            Thread.Sleep(1000);

            dynamic secondValue = _cpuCounter.NextValue();

            return secondValue;
        }

        /// <summary>
        /// The windows service start method
        /// </summary>
        public void Start()
        {
            StartUp();

            _timer = new Timer(TimerCallback, null, 0, 2000);
        }

        /// <summary>
        /// Starts a new thread with the webcrawler
        /// </summary>
        /// <param name="webCrawler">The WebCrawler</param>
        private static void StartCrawling(WebCrawler webCrawler)
        {
            Thread thread = new Thread(webCrawler.Run);
            thread.Start();
        }

        /// <summary>
        /// The windows service stop method
        /// </summary>
        public void Stop()
        {
            _timer.Dispose();
            _crawler.Stop = true;

            ToStorage.WriteEnd();
        }
    }
}
