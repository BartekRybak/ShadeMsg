/*using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using ShadeMsg.Security;

namespace ShadeMsg_Server
{
    /*
     * Current working on change nick storage
     
    class UserFriends_Row
    {
        public string nick;
        public string[] friends;
        public string[] blocked;
        public string[] invites;
        

        public UserFriends_Row() { }

        public UserFriends_Row(string nick, string[] friends,string[] blocked,string[] invites)
        {
            this.nick = nick;
            this.friends = friends;
            this.blocked = blocked;
            this.invites = invites;
        }
    }

    class DB_UserFriends : DataBase
    {
 
        private UserFriends_Row GetFriendsInfo()
        {
            return null;
        }

        /// <summary>
        /// get FriendList field
        private static string GetFriendsAsString(string nick)
        {
            SQLiteConnection sql = DataBase.GetConnection(db_path);
            SQLiteCommand cmd = new SQLiteCommand(sql)
            {
                CommandText = "SELECT * FROM Friends"
            };

            string friends = string.Empty;

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((string)reader["Nick"] == nick)
                    {
                        friends = (string)reader["FriendList"];
                    }
                }
                reader.Close();
            }
            sql.Close();

            return friends;
        }

        private static void SetFriendListField(string nick,string field)
        {

        }

        /// <summary>
        /// Users are friends
        /// work!
        /// </summary>
        public static bool UsersAreFriends(string nick1,string nick2)
        {
            return (FriendExits(nick1, nick2) && (FriendExits(nick2, nick1)));
        }

        /// <summary>
        /// Add Friend
        /// work !
        /// </summary>
        public static void AddFriend(string nick, string friend)
        {
            
        }

        /// <summary>
        /// Its user friend?
        /// work!
        /// </summary>
        public static bool FriendExits(string nick,string friend)
        {
            return false;
        }

        /// <summary>
        /// Dell Friend
        /// work!
        /// </summary>
        public static void DellFriend(string nick,string friend)
        {
           
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
             //   friendsList.Add();
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
*/