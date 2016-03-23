using System.Collections.Generic;

namespace Core
{
    public class TestData
    {
        public static List<Link> GetTestData()
        {
            List<Link> links = new List<Link>();

            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/"));
            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/game/"));
            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/archief/"));
            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/videos/"));
            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/community/"));

            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/game/", "http://insidegamer.nl/game/the-elder-scrolls-v-skyrim/"));
            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/game/", "http://insidegamer.nl/game/tomb-raider-2013/"));
            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/game/", "http://insidegamer.nl/game/random/"));

            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/archief/", "http://insidegamer.nl/archief/random1/"));
            links.Add(new Link("insidegamer.nl", "http://insidegamer.nl/archief/", "http://insidegamer.nl/archief/random2/"));

            return links;
        }
    }
}
