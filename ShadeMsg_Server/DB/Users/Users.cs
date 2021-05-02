using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using ShadeMsg.Security;
using System.IO;

namespace ShadeMsg_Server.DB
{
    partial class DataBase
    {
        public static class Users
        {
            private static readonly string default_friends_table = @"CREATE TABLE IF NOT EXISTS [Friends] (
                            [nick] VARCHAR(2048),
                            [friends] VARCHAR(2048) NULL,
                            [invites_in] VARCHAR(2048) NULL,
                            [invites_out] VARCHAR(2048) NULL,
                            [blocked] VARCHAR(2048) NULL
                            )";

            private static readonly string default_users_table = @"CREATE TABLE IF NOT EXISTS [Users] (
                          [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [nick] VARCHAR(2048)  NULL,
                          [key] VARCHAR(2048)  NULL,
                          [iv] VARCHAR(2048)
                          )";

            public static void TEST()
            {
                Console.WriteLine(ArrayToField(new string[] { "kupa1", "kupa2", "kupa3" }, '+'));
            }

            public static void CreateEmptyTables()
            {
                using (SQLiteConnection sql = GetConnection(db_path))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(default_users_table, sql))
                    {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = default_friends_table;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            #region Users
            public static UserInfo GetUserInfo(string nick, bool withSecureData)
            {
                UserInfo row = new UserInfo();
                string query = "Select * FROM Users WHERE nick='" + nick + "'";

                using (SQLiteConnection sql = GetConnection(db_path))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(query, sql))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    row.nick = nick;
                                    row.id = reader.GetInt32(0).ToString();

                                    if (withSecureData)
                                    {
                                        row.key = reader.GetString(2);
                                        row.iv = reader.GetString(3);
                                    }
                                }
                            }
                        }
                    }
                }
                return row;
            }

            public static void CreateNew(string nick, string password)
            {
                byte[] iv = Encryption.GenerateIV();
                byte[] userKey = Encryption.CreateKey(password);
                string crypted_nick = Convert.ToBase64String(Encryption.Encrypt(nick, userKey, iv));

                using (SQLiteConnection sql = GetConnection(db_path))
                {
                    string query = "INSERT INTO Users (nick,key,iv) VALUES ('" + nick + "','" + crypted_nick + "','" + Convert.ToBase64String(iv) + "')";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, sql))
                    {
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "INSERT INTO Friends (nick,friends,invites_in,invites_out,blocked) VALUES ('" + nick + "','','','','')";
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public static bool UserExits(string nick)
            {
                string query = "SELECT * FROM Users WHERE nick='" + nick + "'";

                using (SQLiteConnection sql = GetConnection(db_path))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(query,sql))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }
                }
            }

            public static bool GetAuth(string nick, string password)
            {
                if(UserExits(nick))
                {
                    UserInfo user = GetUserInfo(nick, true);
                    byte[] iv = Convert.FromBase64String(user.iv);
                    byte[] key = Encryption.CreateKey(password);
                    byte[] cryptedNick = Convert.FromBase64String(user.key);

                    try
                    {
                        return (nick == Encryption.Decrypt(cryptedNick, key, iv));
                    }
                    catch { return false;}
                } else { return false; }
            }
            #endregion

            #region Friends
            public static FriendsInfo GetFriendsInfo(string nick)
            {
                FriendsInfo friendsRow = new FriendsInfo();
                if(UserExits(nick))
                {
                    using (SQLiteConnection sql = GetConnection(db_path))
                    {
                        string query = "SELECT * FROM Friends WHERE nick='"+ nick +"'";
                        using (SQLiteCommand cmd = new SQLiteCommand(query, sql))
                        {
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                if(reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        friendsRow.nick = nick;
                                        friendsRow.friends = FieldToArray(reader.GetString(1), '+');
                                        friendsRow.invites_in = FieldToArray(reader.GetString(2),'+');
                                        friendsRow.invites_out = FieldToArray(reader.GetString(3), '+');
                                        friendsRow.blocked = FieldToArray(reader.GetString(4),'+');
                                    }
                                }
                            }
                        }
                    }
                }
                return friendsRow;
            }
            #endregion
        }
    }
}
