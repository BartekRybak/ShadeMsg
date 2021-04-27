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

        /// <summary>
        /// Create empty table
        /// </summary>
        public static void CreateEmptyTable()
        {
            SQLiteConnection sql = GetConnection(db_path);

            SQLiteCommand cmd = new SQLiteCommand(sql) { 
                CommandText = default_table 
                };
            cmd.ExecuteNonQuery();
            sql.Close();
        }

        /// <summary>
        /// get FriendList field
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
        public static void DellFriend(string nick,string friend)
        {
            if(!FriendExits(nick,friend)) { return; }

            SQLiteConnection sql = GetConnection(db_path);
            string friends = GetFriendsAsString(nick);
            int friend_id = DB_Users.GetUserID(nick);
            List<int> intFriends = new List<int>();

            foreach(string f in friends.Split('+'))
            {
                intFriends.Add(Convert.ToInt32(f));
            }

            for(int i=0; i < intFriends.ToArray().Length;i++)
            {
                if(intFriends[i] == friend_id)
                {
                    intFriends.RemoveAt(i);
                }
            }

            friends = string.Empty;
            if (intFriends.Count <= 0)
            {
                friends = friend_id.ToString();
            }
            else
            {
                foreach (int f in intFriends)
                {
                    friends += f.ToString() + '+';
                }
                friends += friend_id.ToString();
            }

            SQLiteCommand cmd = new SQLiteCommand("UPDATE Friends SET FriendList='" + friends + "' WHERE Nick='" + Encryption.CreateMD5(nick) + "'", sql);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get Friends List
        /// </summary>
        public static string[] GetFriendsList(string nick)
        {
            string friends = GetFriendsAsString(nick);
            List<string> friendsList = new List<string>();

            foreach(string f in GetFriendsAsString(nick).Split('+'))
            {
                friendsList.Add();
            }

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
