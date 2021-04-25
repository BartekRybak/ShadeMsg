using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace ShadeMsg_Server.DataBase
{
    class DB_Friends : DB
    {
        private static readonly string db_path = DB_PATHS["Users"];

        private static readonly string default_table = @"CREATE TABLE IF NOT EXITS [Friends] (
                                [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                [Friends] VARCHAR(2048) NULL,
                                [Blocked] VARCHAR(2048) NULL
                                )";

        /// <summary>
        /// Delete old and create new table from default query
        /// </summary>
        public override void ResetTable(string dbName, string table, string tablequery)
        {
            base.ResetTable(dbName, table, tablequery);
        }

        /// <summary>
        /// Add Friend
        /// </summary>
        public static void AddFriend(string nick,string friendNick)
        {

        }

        /// <summary>
        /// Dell Friend
        /// </summary>
        public static void DellFriend(string nick,string fiendNick)
        {

        }

        /// <summary>
        /// Get Friends List
        /// </summary>
        public static string[] GetFriendsList(string nick)
        {
            return null;
        }

        /// <summary>
        /// Block Friend
        /// </summary>
        public static void BlockFriend(string nick,string friendNick)
        {

        }

        /// <summary>
        /// Unblock Friend
        /// </summary>
        public static void UnblockFriend(string nick,string friendNick)
        {

        }
    }
}
