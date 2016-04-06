using System.Collections.Generic;
using Core.Data;

namespace Core
{
    public class TestData
    {
        public static List<Link> GetTestData()
        {
            List<Link> links = new List<Link>
            {
                new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/game/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/archief/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/videos/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/community/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/", "http://insidegamer.nl/communityssss/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/game/",
                    "http://insidegamer.nl/game/the-elder-scrolls-v-skyrim/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/game/", "http://insidegamer.nl/game/tomb-raider-2013/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/game/", "http://insidegamer.nl/game/random/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/archief/", "http://insidegamer.nl/archief/random1/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/archief/", "http://insidegamer.nl/archief/random2/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/archief/random1/",
                    "http://insidegamer.nl/archief/random1/ddddd"),
                new Link("insidegamer.nl", "http://insidegamer.nl/archief/random1/",
                    "http://insidegamer.nl/archief/random2/ccccc"),
                new Link("insidegamer.nl", "http://insidegamer.nl/communityssss/", "http://insidegamer.nl/dsadad/"),
                new Link("insidegamer.nl", "http://insidegamer.nl/communityssss/",
                    "http://insidegamer.nl/commuasdasdnityssss/")
            };
            
            return links;
        }
    }
}
