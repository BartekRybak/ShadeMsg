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

        /// <summary>
        /// Add Friend
        /// </summary>
        public static void AddFriend(string nick, string friend)
        {
            SQLiteConnection sql = GetConnection(db_path);
            SQLiteCommand cmd = new SQLiteCommand(sql)
            {
                CommandText = "SELECT * FROM Friends"
            };

            List<string> OldfriendList = new List<string>();

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((string)reader["Nick"] == Encryption.CreateMD5(nick))
                    {
                        string friends = (string)reader["FriendList"];

                        foreach (string f in friends.Split('+'))
                        {
                            if (f != string.Empty)
                            {
                                OldfriendList.Add(f);
                            }

                        }
                    }
                }
                reader.Close();
            }
            string newFriendsString = string.Empty;

            if(OldfriendList.Count >= 0)
            {
                foreach (string oldF in OldfriendList.ToArray())
                {
                    newFriendsString += oldF + "+";
                }
            }

            newFriendsString += DB_Users.GetUserID(friend);

            cmd.CommandText = "UPDATE Friends SET FriendList='" + newFriendsString + "' WHERE Nick='" + Encryption.CreateMD5(nick) + "'";
            cmd.ExecuteNonQuery();
            sql.Close();
            Console.WriteLine("TEST DONE");
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
            SQLiteConnection sql = GetConnection(db_path);
            SQLiteCommand cmd = new SQLiteCommand(sql) {
                CommandText = "SELECT * FROM Friends"
            };

            List<int> friendList = new List<int>();

            using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    if((string)reader["Nick"] == Encryption.CreateMD5(nick))
                    {
                        string friends = (string)reader["FriendList"];

                        foreach(string f in friends.Split(','))
                        {
                            if(f != string.Empty)
                            {
                                friendList.Add(Convert.ToInt32(f));
                            }
                            
                        }
                    }
                }
                reader.Close();
            }
            sql.Close();
            return friendList.ToArray();
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
