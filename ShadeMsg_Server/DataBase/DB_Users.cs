using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using ShadeMsg.Security;
using System.IO;

namespace ShadeMsg_Server.DataBase
{
    class DB_Users : DB
    {
        private static readonly string db_path = db_path = "DataBase/Users.db";

        private static readonly string default_table = @"CREATE TABLE IF NOT EXISTS [Users] (
                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [Nick] VARCHAR(2048)  NULL,
                          [Key] VARCHAR(2048)  NULL,
                          [IV] VARCHAR(2048)
                          )";

        public static void CreateEmptyTable()
        {
            SQLiteConnection sql = GetConnection(db_path);

            SQLiteCommand cmd = new SQLiteCommand(sql)
            {
                CommandText = default_table
            };
            cmd.ExecuteNonQuery();
            sql.Close();
        }

        /// <summary>
        /// Create new user
        /// Register
        /// </summary>
        public static void CreateNewUser(string nick,string password)
        {
            nick = Encryption.CreateMD5(nick);

            byte[] iv = Encryption.GenerateIV();
            byte[] userKey = Encryption.CreateKey(password);
            string crypted_nick = Convert.ToBase64String(Encryption.Encrypt(nick, userKey, iv));

            SQLiteConnection sql = GetConnection(db_path);
            SQLiteCommand cmd = new SQLiteCommand(sql)
            {
                CommandText = "INSERT INTO Users (Nick,Key,IV) VALUES ('" + nick + "','" + crypted_nick + "','" + Convert.ToBase64String(iv) + "')"
            };
            cmd.ExecuteNonQuery();
            


            // Friends
            cmd.CommandText = "INSERT INTO Friends (Nick,FriendList,Blocked) VALUES ('"+ nick +"','','')";
            cmd.ExecuteNonQuery();
            
            sql.Close();
        }

        /// <summary>
        /// Check user exits
         /// </summary>
        public static bool UserExits(string nick)
        {
            nick = Encryption.CreateMD5(nick);

            SQLiteConnection sql = GetConnection(db_path);
            SQLiteCommand cmd = new SQLiteCommand(sql)
            {
                CommandText = "Select * FROM Users"
            };

            using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    if((string)reader["nick"] == nick)
                    {
                        sql.Close();
                        return true;
                    }
                }
                sql.Close();
                return false;
            }
            
        }

        /// <summary>
        /// Auth user
        /// </summary>
        public static bool GetAuth(string nick,string password)
        {
            nick = Encryption.CreateMD5(nick);
            SQLiteConnection sql = GetConnection(db_path);
            SQLiteCommand cmd = new SQLiteCommand(sql)
            {
                CommandText = "Select * FROM Users"
            };

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if ((string)reader["nick"] == nick)
                    {
                        byte[] iv = Convert.FromBase64String((string)reader["IV"]);
                        byte[] cryptedNick = Convert.FromBase64String((string)reader["Key"]);
                        byte[] key = Encryption.CreateKey(password);
                        string encrypted_nick = string.Empty;

                        try
                        {
                            encrypted_nick = Encryption.Decrypt(cryptedNick, key, iv);
                        }
                        catch
                        {
                            sql.Close();
                            return false;
                        }

                        if(encrypted_nick == nick)
                        {
                            sql.Close();
                            return true;
                        }
                    }
                }
                sql.Close();
                return false;
            }
        }

        /// <summary>
        /// Loggin
        /// </summary>
        public static bool LogIn(string nick,string password)
        {
            return GetAuth(Encryption.CreateMD5(nick), password);
        }
 

        /// <summary>
        /// Get user ID
        /// </summary>
        public static int GetUserID(string nick)
        {
            SQLiteConnection sql = GetConnection(db_path);

            SQLiteCommand cmd = new SQLiteCommand(sql) { 
                CommandText = "SELECT * FROM Users" 
            };
            
            using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    if((string)reader["Nick"] == Encryption.CreateMD5(nick))
                    {
                        int id = Convert.ToInt32(reader["ID"]);
                        reader.Close();
                        sql.Close();
                        
                        return id;
                    }
                }
            }
            sql.Close();
            return 0;
        }  
    }
}
