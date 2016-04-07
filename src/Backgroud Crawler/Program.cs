using Backgroud_Crawler.Service;
using Topshelf;

namespace Backgroud_Crawler
{
    /// <summary>
    /// Main startup class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">Startup parameters</param>
        private static void Main(string[] args)
        {
            //Run application as service
            HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.Service<CrawlerController>(serviceConfigurator =>
                {
                    serviceConfigurator.ConstructUsing(() => new CrawlerController());
                    serviceConfigurator.WhenStarted(myService => myService.Start());
                    serviceConfigurator.WhenStopped(myService => myService.Stop());
                });

                hostConfigurator.SetDisplayName("Visual_Background_Crawler");
                hostConfigurator.SetServiceName("Visual_Background_Crawler");
                hostConfigurator.SetDescription("Crawls the web for links");
                hostConfigurator.RunAsLocalSystem();
            });
        }
    }
}
