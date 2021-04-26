using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using ShadeMsg.Security;

namespace ShadeMsg_Server.DataBase
{
    class DB_Friends : DB
    {
        private static readonly string db_path = "DataBase/Users.db";

        private static readonly string default_table = @"CREATE TABLE IF NOT EXISTS [Friends] (
                                [Nick] VARCHAR(2048),
                                [FriendList] VARCHAR(2048) NULL,
                                [Blocked] VARCHAR(2048) NULL
                                )";
        public static void Reset()
        {
            ResetTable(db_path, "Friends", default_table);
        }

        public static void CreateEmptyTable()
        {
            SQLiteConnection sql = GetConnection(db_path);

            SQLiteCommand cmd = new SQLiteCommand(sql) { 
                CommandText = default_table 
                };
            cmd.ExecuteNonQuery();
            sql.Close();
            

            Console.WriteLine("Done!");
        }

        private static string GetFriendsAsString(string nick)
        {
            SQLiteConnection sql = GetConnection(db_path);
            SQLiteCommand cmd = new SQLiteCommand(sql)
            {
                CommandText = "SELECT * FROM Friends"
            };

            string friends = string.Empty;

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((string)reader["Nick"] == Encryption.CreateMD5(nick))
                    {
                        friends = (string)reader["FriendList"];
                    }
                }
                reader.Close();
            }
            sql.Close();

            return friends;
        }

        /// <summary>
        /// Add Friend
        /// </summary>
        public static void AddFriend(string nick, string friend)
        {
            SQLiteConnection sql = GetConnection(db_path);
            List<string> OldfriendList = new List<string>();
            int newFriendId = DB_Users.GetUserID(friend);
            string friends = GetFriendsAsString(nick);
            foreach (string f in friends.Split('+'))
            {
                if (f != string.Empty)
                {
                    OldfriendList.Add(f);
                }

            }
            string newFriendsString = string.Empty;

            if(OldfriendList.Count >= 0)
            {
                foreach (string oldF in OldfriendList.ToArray())
                {
                    if(Convert.ToInt32(oldF) != newFriendId)
                    {
                        newFriendsString += oldF + "+";
                    }
                }
            }

            newFriendsString += newFriendId;
            SQLiteCommand cmd = new SQLiteCommand(sql);
            cmd.CommandText = "UPDATE Friends SET FriendList='" + newFriendsString + "' WHERE Nick='" + Encryption.CreateMD5(nick) + "'";
            cmd.ExecuteNonQuery();
            sql.Close();
        }

        /// <summary>
        /// Its user friend?
        /// </summary>
        public static bool FriendExits(string nick,string friend)
        {
            string friends = GetFriendsAsString(nick);
            int friend_id = DB_Users.GetUserID(friend);

            bool exits = false;
            foreach(string f in friends.Split('+'))
            {
                if(Convert.ToInt32(f) == friend_id) { exits = true; }
            }
            return exits;
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
        public static int[] GetFriendsList(string nick)
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
