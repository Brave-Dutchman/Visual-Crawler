using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace Core
{
    public static class Storage
    {
        private static SQLiteConnection _dbConnection;

        public static void Connect(string filename)
        {         
                if (!File.Exists(filename))
                {
                    SQLiteConnection.CreateFile(filename);
                    Debug.WriteLine(string.Format("File {0} Created.", filename));

                }
            _dbConnection = new SQLiteConnection(string.Format("Data Source={0};Version=4", filename));
        }
    }
}
