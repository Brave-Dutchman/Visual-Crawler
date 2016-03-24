using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace Core
{
    /// <summary>
    ///     Storage class.
    ///     Used to read/write data from SQLite database.
    /// </summary>
    public static class Storage
    {
        //Fields
        private const string DB_FILE = "Storage.sqlite"; //Filename for database
        private const string LINKNAME = "Link"; //Value for Link tablename
        private const string CRAWLEDLINKNAME = "CrawledLink"; //Value for CrawledLink tablename
        private static SQLiteConnection _dbConnection; //Connection to database
        private static string _dbConn; //Connection string

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
            _dbConn = string.Format("Data Source={0};Version=3", DB_FILE);
            if (!File.Exists(DB_FILE))
            {
                CreateDatabaseFile();
            }
            _dbConnection = new SQLiteConnection(_dbConn);
            _dbConnection.Open();
            CheckCreateTable(LINKNAME, 0);
            CheckCreateTable(CRAWLEDLINKNAME, 1);
        }

        /// <summary>
        ///     Close the database connection.
        /// </summary>
        private static void Disconnect()
        {
            if (_dbConnection.State != ConnectionState.Closed)
            {
                _dbConnection.Close();
            }
        }

        /// <summary>
        ///     Method to create the database file.
        /// </summary>
        private static void CreateDatabaseFile()
        {
            SQLiteConnection.CreateFile(DB_FILE);
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

                    ExecuteQuery(sql);
                }
            }
            //catch
            {
                //return false;
            }
            return true;
        }

        /// <summary>
        ///     Write Links to database.
        /// </summary>
        /// <param name="links">List of Links</param>
        public static void WriteLinks(List<Link> links)
        {
            foreach (Link link in links)
            {
                ExecuteQuery(CreateWriteLinkQuery(link));
            }
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
        ///     Create a query for reading from the database
        /// </summary>
        /// <param name="table">Tablename</param>
        /// <returns>SQL query</returns>
        private static string CreateReadQuery(string table)
        {
            return string.Format("select * from {0}", table);
        }

        /// <summary>
        ///     Write CrawledLinks to database.
        /// </summary>
        /// <param name="links">List of CrawledLinks</param>
        public static void WriteLinks(List<CrawledLink> links)
        {
            foreach (CrawledLink link in links)
            {
                ExecuteQuery(CreateWriteCrawledLinkQuery(link));
            }
        }

        /// <summary>
        ///     Construct a SQL query string for Links.
        /// </summary>
        /// <param name="link">Link object</param>
        /// <returns>SQL query string</returns>
        private static string CreateWriteLinkQuery(Link link)
        {
            return string.Format(
                "insert into {0} (Host, Origin, Destiny, TimesOnPage) values ('{1}', '{2}', '{3}', {4})", LINKNAME,
                link.Host,
                link.From, link.To, link.TimesOnPage);
        }

        /// <summary>
        ///     Construct a SQL query string for CrawledLinks.
        /// </summary>
        /// <param name="link">CrawledLink object</param>
        /// <returns>SQL query string</returns>
        private static string CreateWriteCrawledLinkQuery(CrawledLink link)
        {
            return string.Format(
                "insert into {0} (Link) values ('{1}')", CRAWLEDLINKNAME, link.Link);
        }

        /// <summary>
        ///     Executes a query using a SQL query string.
        /// </summary>
        /// <param name="sql">SQL query string</param>
        private static void ExecuteQuery(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        ///     Create a new SQLite reader.
        /// </summary>
        /// <param name="sql">SQL query string</param>
        /// <returns>SQLite Reader</returns>
        private static SQLiteDataReader ExecuteReader(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, _dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
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
        ///     Read CrawledLinks from database and return them in a list.
        /// </summary>
        /// <param name="reader">SQL Reader</param>
        /// <returns>List of CrawledLinks</returns>
        private static List<CrawledLink> ReadCrawledLinks(SQLiteDataReader reader)
        {
            List<CrawledLink> list = new List<CrawledLink>();
            while (reader.Read())
            {
                CrawledLink link = new CrawledLink(reader["Link"].ToString());
                list.Add(link);
            }
            return list;
        }
    }
}