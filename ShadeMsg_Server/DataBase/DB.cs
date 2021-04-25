using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace ShadeMsg_Server.DataBase
{
    class DB
    {
        private static readonly string DB_DIRECTORY = "DataBase/";

        public static Dictionary<string, string> DB_PATHS = new Dictionary<string, string>() {
            { "User",DB_DIRECTORY }
        };

        /// <summary>
        /// Get database connection
        /// </summary>
        protected static SQLiteConnection GetConnection(string databasePath)
        {
            SQLiteConnection sql = new SQLiteConnection("data source=" + databasePath);
            sql.Open();
            return sql;
        }

        /// <summary>
        /// Reset table to empty default
        /// </summary>
        public virtual void ResetTable(string dbName,string table,string tablequery)
        {
            DropTable(dbName, table);
            CreateNewTable(dbName, tablequery);
        }

        /// <summary>
        /// Create new Database
        /// </summary>
        protected static void CreateNewDatabase(string dbName)
        {
            if(File.Exists(dbName)) { File.Delete(dbName); }
            SQLiteConnection.CreateFile(dbName);
        }

        /// <summary>
        /// Create new table
        /// </summary>
        protected static void CreateNewTable(string db_name,string query)
        {
            SQLiteConnection sql = GetConnection(db_name);
            SQLiteCommand cmd = new SQLiteCommand(sql) { CommandText = query };
            cmd.ExecuteNonQuery();
            sql.Close();
        }

        /// <summary>
        /// Drop(Delete) table
        /// </summary>
        protected static void DropTable(string dbName,string tableName)
        {
            SQLiteConnection sql = GetConnection(dbName);
            SQLiteCommand cmd = new SQLiteCommand(sql) { CommandText = "DROP TABLE " + tableName };
            cmd.ExecuteNonQuery();
            sql.Close();
        }
    }
}
