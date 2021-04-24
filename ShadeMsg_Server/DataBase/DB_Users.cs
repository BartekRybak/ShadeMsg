using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using ShadeMsg.Security;
using System.IO;

namespace ShadeMsg_Server.DataBase
{
    class DB_Users
    {
        private static readonly string db_path = "DataBase/Users.db";

        private static readonly string table_query = @"CREATE TABLE IF NOT EXISTS [Users] (
                          [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                          [Nick] VARCHAR(2048)  NULL,
                          [Key] VARCHAR(2048)  NULL,
                          [IV] VARCHAR(2048)
                          )";

        /// <summary>
        /// Delete old and create new users database
        /// </summary>
        public static void CreateNewDatabase()
        {
            if (File.Exists(db_path)) { File.Delete(db_path); }
            SQLiteConnection.CreateFile(db_path);

            SQLiteCommand cmd = new SQLiteCommand(GetConnection())
            { CommandText = table_query };
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="nick">user nick</param>
        /// <param name="password">user password</param>
        public static void CreateNewUser(string nick,string password)
        {
            nick = Encryption.CreateMD5(nick);

            byte[] iv = Encryption.GenerateIV();
            byte[] userKey = Encryption.CreateKey(password);
            string crypted_nick = Convert.ToBase64String(Encryption.Encrypt(nick, userKey, iv));

            SQLiteConnection sql = GetConnection();
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

            SQLiteConnection sql = GetConnection();
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
            SQLiteConnection sql = GetConnection();
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

        public static bool LogIn(string nick,string password)
        {
            string _nick = Encryption.CreateMD5(nick);

            if(GetAuth(nick,password))
            {
                return true;
            }
            return false;
        }

        public static int GetUserID(string nick)
        {
            SQLiteConnection sql = GetConnection();

            SQLiteCommand cmd = new SQLiteCommand(sql) { 
                CommandText = "SELECT * FROM Users" 
            };
            
            using(SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    if((string)reader["Nick"] == Encryption.CreateMD5(nick))
                    {
                        return Convert.ToInt32(reader["ID"]);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Get database connection
        /// </summary>
        private static SQLiteConnection GetConnection()
        {
            SQLiteConnection sql = new SQLiteConnection("data source=" + db_path);
            sql.Open();
            return sql;
        }
    }
}
