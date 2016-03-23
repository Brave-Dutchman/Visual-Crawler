using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace Core
{
    public static class Storage
    {
        private const string DB_FILE = "Storage.sqlite";
        private const string LINKNAME = "Link";
        private const string CRAWLEDLINKNAME = "CrawledLink";
        private static SQLiteConnection _dbConnection;

        public static void Connect()
        {
            if (!File.Exists(DB_FILE))
            {
                CreateDatabaseFile();
            }
            _dbConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3", DB_FILE));
            _dbConnection.Open();
            CheckCreateTable(LINKNAME, 0);
            CheckCreateTable(CRAWLEDLINKNAME, 1);
        }


        private static void CreateDatabaseFile()
        {
            SQLiteConnection.CreateFile(DB_FILE);
            Debug.WriteLine(string.Format("File {0} Created.", DB_FILE));
        }

        private static bool CheckCreateTable(string tablename, int type)
        {
            //try
            {
                
                string sql = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}';",
                    tablename);
                SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
                if (!command.ExecuteReader().HasRows)
                {
                    if (type == 0) //Link datatype
                    {
                        sql =
                            string.Format(
                                "CREATE TABLE {0} (Host VARCHAR(255), Origin VARCHAR(255), Destiny VARCHAR(255), TimesOnPage INT)",
                                tablename);
                    }
                    else if (type == 1) //CrawledLink
                    {
                        sql = string.Format("CREATE TABLE {0} (Link VARCHAR(255));", tablename);
                    }
                    else
                    {
                        return false;
                    }

                    command = new SQLiteCommand(sql, _dbConnection);
                    command.ExecuteNonQuery();
                    
                }
             
            }
            //catch
            {
                //return false;
            }
            return true;
        }

        public static void WriteLinks(List<Link> links)
        {
            foreach (Link link in links)
            {
                WriteRow(CreateLinkQuery(link));
            }
        }


        private static string CreateLinkQuery(Link link)
        {
            return string.Format(
                       "insert into {0} (Host, Origin, Destiny, TimesOnPage) values ('{1}', '{2}', '{3}', {4})", LINKNAME, link.Host,
                       link.From, link.To, link.TimesOnPage);
        }

        private static string CreateCrawledLinkQuery(CrawledLink link)
        {
            return string.Format(
                        "insert into {0} (Link) values ({1})", CRAWLEDLINKNAME, link.Link);
        }

        private static void WriteRow(string sql)
        {  
                SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
                command.ExecuteNonQuery();                   
        }
    }
}