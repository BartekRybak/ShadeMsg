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
        private static readonly string db_path = DB_PATHS["Users"];

        private static readonly string default_table = @"CREATE TABLE IF NOT EXISTS [Users] (
                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [Nick] VARCHAR(2048)  NULL,
                          [Key] VARCHAR(2048)  NULL,
                          [IV] VARCHAR(2048)
                          )";

        /// <summary>
        /// Delete old and create new table from default query
        /// </summary>
        public override void ResetTable(string dbName, string table, string tablequery)
        {
            base.ResetTable(dbName, table, tablequery);
        }

        /// <summary>
        /// Create new user
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
                CommandText = "INSERT INTO Users (Nick,Key,IV) Values ('" + nick + "','" + crypted_nick + "','" + Convert.ToBase64String(iv) + "')"
            };

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
                        sql.Close();
                        return id;
                    }
                }
            }
            return 0;
        }  
    }
}
