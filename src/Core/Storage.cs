using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Core.Objects;

namespace Core
{
    /// <summary>
    ///     Storage class.
    ///     Used to read/write data from SQLite database.
    /// </summary>
    public static class Storage
    {
        //Fields
        private const string DB_FILE = "VisualWebCrawler.sqlite"; //Filename for database
        private const string LINKNAME = "Link"; //Value for Link tablename
        private const string CRAWLEDLINKNAME = "CrawledLink"; //Value for CrawledLink tablename
        private static SQLiteConnection _dbConnection; //Connection to database
        private static string _dbConn; //Connection string
        private static string _filePath; //The Database filename with complete path
        private static bool _connectionState;

        /// <summary>
        ///     Constructor
        /// </summary>
        static Storage()
        {
            Enable();
        }

        /// <summary>
        ///     Initializes the database.
        ///     Checks if database file exists, and creates the file in case of absence.
        ///     Also creates the needed tables when creating a new database.
        ///     Opens the connection.
        /// </summary>
        private static void Enable()
        {
            _filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + DB_FILE;
            _dbConn = string.Format("Data Source={0};Version=3;Compress=True;", _filePath);

            if (!File.Exists(_filePath))
            {
                CreateDatabaseFile();
            }
            Connect();
            CheckCreateTable(LINKNAME, 0);
            CheckCreateTable(CRAWLEDLINKNAME, 1);
        }

        /// <summary>
        /// Connect to database.
        /// </summary>
        private static void Connect()
        {
            if (_connectionState == false)
            {
                _dbConnection = new SQLiteConnection(_dbConn);
                _dbConnection.Open();
                _connectionState = true;
            }
        }

        /// <summary>
        ///     Close the database connection.
        /// </summary>
        private static void Disconnect()
        {
            if (_connectionState)
            {
                _dbConnection.Close();
                _connectionState = false;
            }
        }

        /// <summary>
        ///     Method to create the database file.
        /// </summary>
        private static void CreateDatabaseFile()
        {
            SQLiteConnection.CreateFile(_filePath);
            Debug.WriteLine(string.Format("File {0} Created.", DB_FILE));
        }

        /// <summary>
        ///     Check and create the tables.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        /// <param name="type">Tabletype: 0 for Link, 1 for CrawledLink</param>
        /// <returns>Succes/Failure</returns>
        private static bool CheckCreateTable(string tablename, int type)
        {
            string sql = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}';", tablename);
            using (SQLiteCommand command = new SQLiteCommand(sql, _dbConnection))
            {
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
                        sql = string.Format("CREATE TABLE {0} (Link VARCHAR(255), IsCrawled INT);", tablename);
                    }
                    else
                    {
                        return false;
                    }

                    ExecuteQuery(sql);
                }
            }

            return true;
        }

        /// <summary>
        ///     Fetch the Links from the database and return them in a list.
        /// </summary>
        /// <returns>List of Links</returns>
        public static List<Link> GetLinks()
        {
            return ReadLinks(ExecuteReader(CreateReadQuery(LINKNAME)));
        }

        /// <summary>
        ///     Fetch the CrawledLinks from the database and return them in a list.
        /// </summary>
        /// <returns>List of CrawledLinks</returns>
        public static List<CrawledLink> GetCrawledLinks()
        {
            return ReadCrawledLinks(ExecuteReader(CreateReadQuery(CRAWLEDLINKNAME)));
        }

        /// <summary>
        /// Convers bool to int
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static int ConvertBoolToInt(bool input)
        {
            return input ? 1 : 0;
        }

        /// <summary>
        /// Converts int to bool
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static bool ConvertIntToBool(int input)
        {
            return input == 1;
        }

        /// <summary>
        ///     Create a query for reading from the database
        /// </summary>
        /// <param name="table">Tablename</param>
        /// <returns>SQL query</returns>
        private static string CreateReadQuery(string table)
        {
            return string.Format("select * from {0}", table);
        }

        /// <summary>
        ///     Create a query for reading from the database
        /// </summary>
        /// <param name="table">Tablename</param>
        /// <returns>SQL query</returns>
        private static string CreateNotCrawledLinkReadQuery(string table)
        {
            return string.Format("select * from {0} where IsCrawled=0 LIMIT 100", table);
        }

        /// <summary>
        ///     Write Links to database.
        /// </summary>
        /// <param name="links">List of Links</param>
        public static void WriteLinks(List<Link> links)
        {
            using (SQLiteCommand command = new SQLiteCommand("begin", _dbConnection))
            {
                command.ExecuteNonQuery();
            }
            foreach (Link link in links)
            {
                //if (!CheckDoubles(link))
                {
                    ExecuteQuery(CreateWriteLinkQuery(link));
                }
            }
            using (SQLiteCommand command = new SQLiteCommand("end", _dbConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Write CrawledLinks to database.
        /// </summary>
        /// <param name="links">List of CrawledLinks</param>
        public static void WriteLinks(List<CrawledLink> links)
        {
            using (SQLiteCommand command = new SQLiteCommand("begin", _dbConnection))
            {
                command.ExecuteNonQuery();
            }
            foreach (CrawledLink link in links)
            {
                //if (!CheckDoubles(link))
                {
                    ExecuteQuery(CreateWriteCrawledLinkQuery(link));
                }
            }
            using (SQLiteCommand command = new SQLiteCommand("end", _dbConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Check for double Link and CrawledLink entries
        /// </summary>
        /// <param name="item">Link/CrawledLink</param>
        /// <returns>Boolean True/False</returns>
        private static bool CheckDoubles(object item)
        {
            if (item.GetType() == typeof (Link))
            {
                return CheckLinksDouble((Link) item);
            }

            if (item.GetType() == typeof (CrawledLink))
            {
                return CheckCrawledLinksDouble(((CrawledLink) item).Link);
            }

            return false;
        }

        /// <summary>
        /// Check for double links
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool CheckLinksDouble(Link item)
        {
            return GetLinks().Any(link => link.From == item.From && link.Host == item.Host && link.To == item.To);
        }

        /// <summary>
        /// Check for double crawled links
        /// </summary>
        /// <param name="itemLink"></param>
        /// <returns></returns>
        public static bool CheckCrawledLinksDouble(string itemLink)
        {
            return GetCrawledLinks().Any(link => link.Link == itemLink);
        }

        /// <summary>
        ///     Construct a SQL query string for Links.
        /// </summary>
        /// <param name="link">Link object</param>
        /// <returns>SQL query string</returns>
        private static string CreateWriteLinkQuery(Link link)
        {
            return
                string.Format("insert into {0} (Host, Origin, Destiny, TimesOnPage) values ('{1}', '{2}', '{3}', {4})",
                    LINKNAME, link.Host, link.From, link.To, link.TimesOnPage);
        }

        /// <summary>
        ///     Construct a SQL query string for CrawledLinks.
        /// </summary>
        /// <param name="link">CrawledLink object</param>
        /// <returns>SQL query string</returns>
        private static string CreateWriteCrawledLinkQuery(CrawledLink link)
        {
            return string.Format("insert into {0} (Link, IsCrawled) values ('{1}', {2})", CRAWLEDLINKNAME, link.Link,
                ConvertBoolToInt(link.IsCrawled));
        }

        /// <summary>
        /// Create query for updating crawled links boolean
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private static string CreateUpdateCrawledLinkQuery(string link)
        {
            return string.Format("update {0} set IsCrawled=1 where Link='{1}'", CRAWLEDLINKNAME, link);
        }

        /// <summary>
        ///     Executes a query using a SQL query string.
        /// </summary>
        /// <param name="sql">SQL query string</param>
        private static async void ExecuteQuery(string sql)
        {
            using (SQLiteCommand command = new SQLiteCommand(sql, _dbConnection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        ///     Create a new SQLite reader.
        /// </summary>
        /// <param name="sql">SQL query string</param>
        /// <returns>SQLite Reader</returns>
        private static SQLiteDataReader ExecuteReader(string sql)
        {
            SQLiteDataReader reader;
            using (SQLiteCommand command = new SQLiteCommand(sql, _dbConnection))
            {
                reader = command.ExecuteReader();
            }
            return reader;
        }

        /// <summary>
        ///     Read Links from database and return them in a list.
        /// </summary>
        /// <param name="reader">SQLite Reader</param>
        /// <returns>List of Links</returns>
        private static List<Link> ReadLinks(SQLiteDataReader reader)
        {
            List<Link> list = new List<Link>();
            while (reader.Read())
            {
                Link link = new Link(reader["Host"].ToString(), reader["Origin"].ToString(),
                    reader["Destiny"].ToString());
                list.Add(link);
            }
            return list;
        }

        /// <summary>
        /// Update the crawled links to crawled=true
        /// </summary>
        /// <param name="links"></param>
        public static void UpdateCrawledLinks(List<string> links)
        {
            using (SQLiteCommand command = new SQLiteCommand("begin", _dbConnection))
            {
                command.ExecuteNonQuery();
            }

            foreach (string link in links)
            {
                ExecuteQuery(CreateUpdateCrawledLinkQuery(link));
            }

            using (SQLiteCommand command = new SQLiteCommand("end", _dbConnection))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        ///     Read CrawledLinks from database and return them in a list.
        /// </summary>
        /// <param name="reader">SQL Reader</param>
        /// <returns>List of CrawledLinks</returns>
        private static List<CrawledLink> ReadCrawledLinks(SQLiteDataReader reader)
        {
            List<CrawledLink> stack = new List<CrawledLink>();

            while (reader.Read())
            {
                CrawledLink link = new CrawledLink(reader["Link"].ToString(),
                    ConvertIntToBool((int) reader["IsCrawled"]));
                stack.Add(link);
            }
            return stack;
        }

        /// <summary>
        /// Returns links that are not crawled
        /// </summary>
        /// <returns></returns>
        public static Stack<CrawledLink> ReadNotCrawledLinks()
        {
            Stack<CrawledLink> crawled = new Stack<CrawledLink>();

            //int count = 0;

            foreach (CrawledLink link in ReadCrawledLinks(ExecuteReader(CreateNotCrawledLinkReadQuery(CRAWLEDLINKNAME))))
                
            {
                crawled.Push(link);
                //if (!link.IsCrawled)
                //{
                //    crawled.Push(link);
                //    count++;
                //}

                //if (count >= 100)
                //{
                //    break;
                //}
            }

            return crawled;
        }
    }
}