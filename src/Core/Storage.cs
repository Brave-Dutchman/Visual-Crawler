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
                ExecuteQuery(CreateLinkQuery(link));
            }
        }

        /// <summary>
        ///     Write CrawledLinks to database.
        /// </summary>
        /// <param name="links">List of CrawledLinks</param>
        public static void WriteLinks(List<CrawledLink> links)
        {
            foreach (CrawledLink link in links)
            {
                ExecuteQuery(CreateCrawledLinkQuery(link));
            }
        }

        /// <summary>
        ///     Construct a SQL query string for Links.
        /// </summary>
        /// <param name="link">Link object</param>
        /// <returns>SQL query string</returns>
        private static string CreateLinkQuery(Link link)
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
        private static string CreateCrawledLinkQuery(CrawledLink link)
        {
            return string.Format(
                "insert into {0} (Link) values ({1})", CRAWLEDLINKNAME, link.Link);
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
    }
}