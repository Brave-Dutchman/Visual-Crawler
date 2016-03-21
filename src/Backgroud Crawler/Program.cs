using Topshelf;

namespace Backgroud_Crawler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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
