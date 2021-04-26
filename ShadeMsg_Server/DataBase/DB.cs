using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace ShadeMsg_Server.DataBase
{
    class DB
    {
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
        protected static void ResetTable(string dbName,string table,string tablequery)
        {
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
    }
}
